using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(DialogueChoice))]
public class DialogueManager : MonoBehaviour
{

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
    public AudioSource ambientAudio;


    [Header("Exit")]
    public GameObject exitPanel;
    bool isExitPanelOpen = false;

    private Story inkStory;
    private bool isTyping = false;
    private bool skipTyping = false;
    private bool blockNextInput = false;

    private string currentLine = ""; //to deal with pause and resume to typetext
    private Coroutine typingCoroutine; //to deal with pause and resume to typetext


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
        isExitPanelOpen = false;
        exitPanel.SetActive(false);
        LoadStory();

    }

    void Update()
    {
        if (blockNextInput)
        {
            blockNextInput = false; // Skip one frame of input
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            if (isExitPanelOpen)
                ResumeDialogue();
            else
                PauseDialogue();

            return;
        }

        if (isExitPanelOpen)
            return;

        if (isTyping)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
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

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
            ContinueStory();
    }


    public void LoadStory()
    {
        // Load saved values from PlayerPrefs
        GameState.LoadAll();
        string savedInkState = PlayerPrefs.GetString("InkState", "");

        // Initialize Ink story
        inkStory = new Story(currentInkJSON.text);

        //check if there's any return point/knot to go back to 
        CheckReturnPoint();

        // Load previous state if available
        if (!string.IsNullOrEmpty(savedInkState))
        {
            inkStory.state.LoadJson(savedInkState);
            Debug.Log("Restored story from saved state.");

            if (!string.IsNullOrEmpty(lastBackgroundTag))
                ChangeEnvironmentBackground(lastBackgroundTag);

            if (!string.IsNullOrEmpty(lastSpeakerTag))
                currentSpeaker = lastSpeakerTag; // FIXED

            if (!string.IsNullOrEmpty(lastCharacter) && !string.IsNullOrEmpty(lastExpressionTag))
                ChangeCharacterExpression(lastCharacter, lastExpressionTag);
        }

        // Init UI & story
        choiceUI.Init(inkStory, this);
        LoadTypingSounds();

        // ✅ Inject saved variables from GameState
        inkStory.variablesState["trust"] = GameState.trust;
        inkStory.variablesState["delusion"] = GameState.delusion;
       

        if (inkStory.canContinue)
        {
            ContinueStory();
        }
        
    }

    public void CheckReturnPoint()
    {
        string returnPoint = GameState.returnPoint;
        if (!string.IsNullOrEmpty(returnPoint))
        {
            Debug.Log("Jumping to knot: " + returnPoint);
            inkStory.ChoosePathString(returnPoint);
        }
    }

    public void ContinueStory()
    {

        if (inkStory.canContinue)
        {
            SaveInkState();
            currentLine = inkStory.Continue().Trim();
            GameState.AddLine(currentSpeaker ?? "Narrator", currentLine);
            GameState.SaveDialogueHistory(); //record this to dialogue history

            List<string> tags = inkStory.currentTags;

            HandleTags(tags);

            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            typingCoroutine = StartCoroutine(TypeText(currentLine));

            return;
        }
        else if (!inkStory.canContinue)
        {

            //check what version to load to by fetching the story progress
            //and load scene accroding to the version

            if (inkStory.variablesState["UIVersion"] != null)
            {
                int uiV = (int)inkStory.variablesState["UIVersion"];

                Debug.Log("hey we got the version from ink, it's : " + uiV.ToString());


                    GameState.uiVersion = uiV;
                    GameState.ResetStoryData();
                    GameState.returnPoint = SetReturnString(uiV);
                    GameState.SaveVersion();
                    //GameState.SaveAll();
                    LoadInterfaceScene();

            }
            else
            {
                Debug.LogWarning("can't fetch UIVersion");
            }

                Debug.Log("Story can not continue, returning to interface");
            return;
        }
        else
        {
            Debug.LogWarning("this shit is broken");
            return; 
        }
        
 
    }

    IEnumerator TypeText(string text)
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

        if (inkStory.currentChoices.Count > 0)
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
                    if (val.ToLower() == "off")
                    {
                        lastCharacter = "off";
                        currentCharacter = "off";
                        visualManager.HideCharacter();
                    }
                    else
                    {
                        lastCharacter = val;
                        currentCharacter = val;
                        visualManager.ShowCharacter(); // Shows character sprite

                    }
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

                case "ambient":
                    PlayAmbient(val);
                    break;

            }
        }
    }

    void PlayAmbient(string clipName)
    {
        if (clipName.ToLower() == "stop")
        {
            ambientAudio.Stop();
            ambientAudio.clip = null;
            return;
        }

        var clip = Resources.Load<AudioClip>($"Audio/Ambient/{clipName}");
        if (clip)
        {
            if (ambientAudio.clip != clip)
            {
                ambientAudio.clip = clip;
                ambientAudio.loop = true;
                ambientAudio.Play();
            }
        }
        else
        {
            Debug.LogWarning($"Ambient audio not found: {clipName}");
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
        if (inkStory != null)
        {
            GameState.inkStateJSON = inkStory.state.ToJson();
            GameState.lastBackground = lastBackgroundTag;
            GameState.lastCharacter = lastCharacter;
            GameState.lastExpression = lastExpressionTag;
            GameState.lastSpeaker = lastSpeakerTag;


            // ✅ SAVE VARIABLES FROM INK
            if (inkStory.variablesState.Contains("trust"))
                GameState.trust = (int)inkStory.variablesState["trust"];
            if (inkStory.variablesState.Contains("delusion"))
                GameState.delusion = (int)inkStory.variablesState["delusion"];


            GameState.SaveAll(); // Assuming this commits to disk or serialization
        }
    }



    string SetReturnString(int uiVersion)
    {
        return "ROUND_" + uiVersion;
    }

    [Tooltip("Loads the Interface scene based on the GameState.uiVersion value")]
    public void LoadInterfaceScene()
    {
        switch (GameState.uiVersion)
        {
            case 1:
                Debug.Log("UIVersion 1 → Version1");
                SceneManager.LoadScene("Version1");
                break;
            case 2:
                Debug.Log("UIVersion 2 → Version2");
                SceneManager.LoadScene("Version2");
                
                break;
            case 3:
                Debug.Log("UIVersion 3 → Version3");
                SceneManager.LoadScene("Version3");
               
                break;
            case 4:
                Debug.Log("UIVersion 4 → Version4");
                GameState.returnPoint = "SPLIT_ENDING"; //rename
                SceneManager.LoadScene("Version4");
                break;
            default:
                Debug.LogWarning("Unknown UI Version");
                break;
        }
    }

    public void PauseDialogue()
    {
        isExitPanelOpen = true;
        exitPanel.SetActive(true);
        choiceUI.SetChoicesInteractable(false);

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
            isTyping = false;
        }
    }

    public void ResumeDialogue()
    {
        isExitPanelOpen = false;
        exitPanel.SetActive(false);
        blockNextInput = true;

        if (!string.IsNullOrEmpty(currentLine) && !isTyping)
        {
            dialogueText.text = ""; // Clear previous partial line
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            typingCoroutine = StartCoroutine(TypeText(currentLine));
        }
        
        if (inkStory.currentChoices.Count > 0)
        {
            choiceUI.DisplayChoices();
            choiceUI.SetChoicesInteractable(true);
        }
    }




}