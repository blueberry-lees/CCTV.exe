using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

[RequireComponent(typeof(DialogueChoice))]
public class DialogueManager : MonoBehaviour
{
    [Header("Save State")]
    [TextArea(3, 10)] public string savedInkStateJSON;

    [Header("Script References")]
    private VisualManager visualManager;
    private DialogueChoice choiceUI;

    [Header("UI")]
    public TMP_Text speakerText;
    public TMP_Text dialogueText;

    [Header("Ink")]
    public TextAsset currentInkJSON;

    [Header("Settings")]
    public float typeSpeed = 0.05f;

    [Header("Audio")]
    public AudioSource typingAudio;
    public AudioSource sfxAudio;

    private Story story;
    private bool isTyping = false;
    private bool skipTyping = false;

    private string currentCharacter = "Narrator";
    private string currentSpeaker = "Narrator";

    private string lastBackgroundTag = "";
    private string lastExpressionTag = "";
    [HideInInspector] public string lastCharacter = "";
    private string lastSpeakerTag = "";

    private Dictionary<string, AudioClip> speakerTypingSounds = new();
    private Dictionary<string, string> speakerColors = new()
    {
        { "Male", "#89CFF0" },
        { "Female", "#FFB6C1" },
        { "Narrator", "#FFFFFF" }
    };

    void Awake()
    {
        choiceUI = GetComponent<DialogueChoice>();
        visualManager = GetComponent<VisualManager>();

        // ResetPlayerPrefs();
    }

    void Start()
    {
        LoadStory();

    }

    void Update()
    {

        savedInkStateJSON = PlayerPrefs.GetString("InkState", "");

        if (isTyping)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                skipTyping = true;
            return;
        }

        if (choiceUI.HasChoices())
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) choiceUI.NavigateChoice(-1);
            else if (Input.GetKeyDown(KeyCode.DownArrow)) choiceUI.NavigateChoice(1);
            else if (Input.GetKeyDown(KeyCode.Return)) choiceUI.ChooseSelectedChoice();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
            ContinueStory();
    }

    public void LoadStory()
    {
        // Load saved values from PlayerPrefs
        LoadPlayerPrefs();
        string savedInkState = PlayerPrefs.GetString("InkState", "");

        // Initialize Ink story
        story = new Story(currentInkJSON.text);

        //check if there's any return point/knot to go back to 
        CheckReturnPoint();

        // Load previous state if available
        if (!string.IsNullOrEmpty(savedInkState))
        {
            story.state.LoadJson(savedInkState);
            Debug.Log("Restored story from saved state.");

            if (!string.IsNullOrEmpty(lastBackgroundTag))
                ChangeEnvironmentBackground(lastBackgroundTag);

            if (!string.IsNullOrEmpty(lastSpeakerTag))
                currentSpeaker = lastSpeakerTag; // FIXED

            if (!string.IsNullOrEmpty(lastCharacter) && !string.IsNullOrEmpty(lastExpressionTag))
                ChangeCharacterExpression(lastCharacter, lastExpressionTag);
        }

        // Init UI & story
        choiceUI.Init(story, this);
        LoadTypingSounds();

        if (story.canContinue)
        {
            ContinueStory();
        }
        else
        {
            Debug.LogWarning("Ink story cannot continue from this point.");
        }
    }

    public void CheckReturnPoint()
    {
        string returnPoint = PlayerPrefs.GetString("ReturnPoint", "");

        if (!string.IsNullOrEmpty(returnPoint))
        {
            Debug.Log("Jumping to knot: " + returnPoint);
            story.ChoosePathString(returnPoint);
        }
    }

    public void ContinueStory()
    {
        if (!story.canContinue)
        {
            SaveInkState();
            Debug.LogWarning("Story can not continue, return to interface1");
            SceneManager.LoadScene("Interface1");
            return;
        }

        SaveInkState();
        string line = story.Continue().Trim();
        if (story.variablesState["progress"] != null)
        {
            string progress = story.variablesState["progress"].ToString();

            if (progress == "Round_1_End")
            {
                PlayerPrefs.SetString("ReturnPoint", "Round_2");
                PlayerPrefs.SetInt("UIVersion", 2); 
                //set int uiversion so TerminalUI cs in Interface1 can respond accordingly
                PlayerPrefs.Save();
                CheckInterfaceVersion();
                return;
            }
        }

        List<string> tags = story.currentTags;

        HandleTags(tags);
        StartCoroutine(TypeText(line));

        Debug.Log("Ink progress value: " + story.variablesState["progress"]);
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";
        skipTyping = false;

        float speed = GetFloatFromInk("speed", typeSpeed);
        float delay = GetFloatFromInk("delay", 0f);
        yield return new WaitForSeconds(delay);

        string colorHex = speakerColors.ContainsKey(currentSpeaker) ? speakerColors[currentSpeaker] : "#FFFFFF";
        string fullText = $"<color={colorHex}>{text}</color>";

        int i = 0;
        while (i < fullText.Length)
        {
            if (skipTyping)
            {
                dialogueText.text = fullText;
                break;
            }

            if (fullText[i] == '<')
            {
                int closing = fullText.IndexOf('>', i);
                if (closing != -1)
                {
                    dialogueText.text += fullText.Substring(i, closing - i + 1);
                    i = closing + 1;
                    continue;
                }
            }

            dialogueText.text += fullText[i++];

            if (typingAudio && speakerTypingSounds.TryGetValue(currentSpeaker.ToLower(), out var clip))
            {
                typingAudio.clip = clip;
                typingAudio.Play();
            }

            yield return new WaitForSeconds(speed);
        }

        isTyping = false;

        if (story.currentChoices.Count > 0)
            choiceUI.DisplayChoices();
    }

    float GetFloatFromInk(string varName, float defaultValue)
    {
        var val = story.variablesState[varName];
        return val switch
        {
            int i => i,
            float f => f,
            double d => (float)d,
            _ => defaultValue
        };
    }

    void LoadTypingSounds()
    {
        foreach (string type in new[] { "Male", "Female", "Narrator" })
        {
            var clip = Resources.Load<AudioClip>($"Audio/Typing/{type}");
            if (clip) speakerTypingSounds[type.ToLower()] = clip;
        }
    }

    private void HandleTags(List<string> tags)
    {
        foreach (string tag in tags)
        {
            var splitTag = tag.Trim().Split(':');
            if (splitTag.Length != 2) continue;

            var key = splitTag[0].Trim().ToLower();
            var val = splitTag[1].Trim();

            switch (key)
            {
                case "speaker":
                    lastSpeakerTag = val;
                    speakerText.text = val;
                    currentSpeaker = val;

                    break;

                case "character":
                    lastCharacter = val; // Save current character
                    currentCharacter = val;

                    break;

                case "expression":
                    lastExpressionTag = val; // Save current expression
                    ChangeCharacterExpression(currentCharacter, val);

                    break;

                case "background":
                    lastBackgroundTag = val; // Save current background
                    ChangeEnvironmentBackground(val);
                    break;

                case "sfx":
                    PlaySFX(val);
                    break;
            }
        }
    }

    void PlaySFX(string clipName)
    {
        var clip = Resources.Load<AudioClip>($"Audio/SFX/{clipName}");
        if (clip) sfxAudio.PlayOneShot(clip);
        else Debug.LogWarning($"SFX not found: {clipName}");
    }

    private void ChangeCharacterExpression(string character, string expression)
    {
        visualManager.ChangeCharacterExpression(character, expression);
    }

    private void ChangeEnvironmentBackground(string backgroundName)
    {
        visualManager.ChangeEnvironmentBackground(backgroundName);
    }

    private void SaveInkState()
    {
        if (story != null)
        {
            string json = story.state.ToJson();
            PlayerPrefs.SetString("InkState", json);
            PlayerPrefs.SetString("LastBackground", lastBackgroundTag);
            PlayerPrefs.SetString("LastCharacter", lastCharacter);
            PlayerPrefs.SetString("LastExpression", lastExpressionTag);
            PlayerPrefs.SetString("LastSpeaker", lastSpeakerTag);


            PlayerPrefs.Save();
            Debug.Log("Saved Ink JSON length: " + json.Length);
        }
    }


    void LoadPlayerPrefs()
    {
        //load saved values from player prefs
        lastBackgroundTag = PlayerPrefs.GetString("LastBackground", "");
        lastCharacter = PlayerPrefs.GetString("LastCharacter", "");
        lastExpressionTag = PlayerPrefs.GetString("LastExpression", "");
        lastSpeakerTag = PlayerPrefs.GetString("lastSpeaker", "");
    }


    void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs reset.");
    }


    public void CheckInterfaceVersion()
    {
        //if round 1 have not complete then interface version is 1
        if (PlayerPrefs.GetInt("UIVersion", 0) == 1)
        {
            Debug.Log("playerpref UIVerion is 1 THEREFORE VER.1");
            SceneManager.LoadScene("Interface1");
        }
        else if (PlayerPrefs.GetInt("UIVersion", 0) == 2)
        {
            Debug.Log("playerpref UIVerion is 2 THEREFORE VER.2");
            SceneManager.LoadScene("Interface2");

        }
        else if (PlayerPrefs.GetInt("UIVersion", 0) == 3)
        {
            Debug.Log("playerpref UIVerion is 3 THEREFORE VER.3");
            SceneManager.LoadScene("Interface3");

        }
        else if (PlayerPrefs.GetInt("UIVersion", 0) == 4)
        {
            Debug.Log("playerpref UIVerion is 3 THEREFORE VER.4");
            SceneManager.LoadScene("Interface4");

        }
        else
        {
            Debug.LogWarning("Version not found");
        }
    }

    }