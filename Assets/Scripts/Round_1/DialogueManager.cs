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
    private float tagTypingSpeed = -1f;
    private float tagDelay = 0f;

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
        string returnPoint = GameState.returnPoint;
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
                GameState.returnPoint = "Round_2";
                GameState.uiVersion = 2;
                GameState.SaveAll();
                CheckInterfaceVersion();
                return;
            }
        }

        List<string> tags = story.currentTags;

        // 🔁 Delay tag handling until after typing is complete
        StartCoroutine(TypeText(line, tags));

        Debug.Log("Ink progress value: " + story.variablesState["progress"]);
    }

    IEnumerator TypeText(string text, List<string> tags)
    {
        isTyping = true;
        dialogueText.text = "";
        skipTyping = false;

        float speed = tagTypingSpeed > 0 ? tagTypingSpeed : typeSpeed;
        float delay = tagDelay > 0 ? tagDelay : 0f;
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

        // ✅ Handle tags after dialogue is fully typed
        HandleTags(tags);

        if (story.currentChoices.Count > 0)
            choiceUI.DisplayChoices();

        tagTypingSpeed = -1f;
        tagDelay = 0f;
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
                    lastCharacter = val;
                    currentCharacter = val;
                    break;

                case "expression":
                    lastExpressionTag = val;
                    ChangeCharacterExpression(currentCharacter, val);
                    break;

                case "background":
                    lastBackgroundTag = val;
                    ChangeEnvironmentBackground(val);
                    break;

                case "sfx":
                    PlaySFX(val);
                    break;

                case "speed":
                    if (float.TryParse(val, out float s)) tagTypingSpeed = s;
                    break;

                case "delay":
                    if (float.TryParse(val, out float d)) tagDelay = d;
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
            GameState.inkStateJSON = story.state.ToJson();
            GameState.lastBackground = lastBackgroundTag;
            GameState.lastCharacter = lastCharacter;
            GameState.lastExpression = lastExpressionTag;
            GameState.lastSpeaker = lastSpeakerTag;

            GameState.SaveAll(); // Assuming this commits to disk or serialization

            Debug.Log("Saved Ink JSON length: " + GameState.inkStateJSON.Length);
        }
    }


    void LoadPlayerPrefs()
    {
        //load saved values from player prefs in GameState
        lastBackgroundTag = GameState.lastBackground;
        lastCharacter = GameState.lastCharacter;
        lastExpressionTag = GameState.lastExpression;
        lastSpeakerTag = GameState.lastSpeaker;
    }


    void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs reset.");
    }


    public void CheckInterfaceVersion()
    {
        switch (GameState.uiVersion)
        {
            case 1:
                Debug.Log("UIVersion 1 → Interface1");
                SceneManager.LoadScene("Interface1");
                break;
            case 2:
                Debug.Log("UIVersion 2 → Interface2");
                SceneManager.LoadScene("Interface2");
                break;
            case 3:
                Debug.Log("UIVersion 3 → Interface3");
                SceneManager.LoadScene("Interface3");
                break;
            case 4:
                Debug.Log("UIVersion 4 → Interface4");
                SceneManager.LoadScene("Interface4");
                break;
            default:
                Debug.LogWarning("Unknown UI Version");
                break;
        }
    }

}