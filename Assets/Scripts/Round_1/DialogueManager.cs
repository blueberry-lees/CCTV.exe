using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static VersionLogic;

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


    private string currentCharacter = "";
    private string currentSpeaker = "Narrator";

    
    //private string lastAmbientTag = "";
    //private string lastBackgroundTag = "";
    //private string lastExpressionTag = "";

    //public string lastCharacter = "";
    //private string lastSpeakerTag = "";
    [HideInInspector] public GamePresentationData presentationState = new GamePresentationData();

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
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.Return) || Input.GetMouseButton(0))
                skipTyping = true;
            return;
        }

        if (choiceUI.HasChoices())
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) choiceUI.NavigateChoice(-1);
            else if (Input.GetKeyDown(KeyCode.DownArrow)) choiceUI.NavigateChoice(1);
            else if (Input.GetKey(KeyCode.Return)) choiceUI.ChooseSelectedChoice();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.Return) || Input.GetMouseButton(0))
            ContinueStory();
    }


    public void LoadStory()
    {
        SaveData data = SaveSystem.LoadGame();
        if (data == null)
        {
            Debug.LogWarning("No save data found.");
            return;
        }

        string savedInkState = data.inkStateJSON;

        // ✅ FIX: assign the loaded presentation state here
        presentationState = data.presentation ?? new GamePresentationData();

        // Initialize Ink story
        inkStory = new Story(currentInkJSON.text);

        // Return point logic
        CheckReturnPoint();

        // Load previous Ink state
        if (!string.IsNullOrEmpty(savedInkState))
        {
            inkStory.state.LoadJson(savedInkState);
            Debug.Log("Restored story from saved state.");
            LoadVisuals();

        }

        // Init UI and sounds
        choiceUI.Init(inkStory, this);
        LoadTypingSounds();

        // Inject tracked variables
        inkStory.variablesState["trust"] = data.trust;
        inkStory.variablesState["delusion"] = data.delusion;

        if (GameState.choicesMadeList != null && GameState.choicesMadeList.Count > 0)
        {
            var list = new Ink.Runtime.InkList("all_directions", inkStory);
            foreach (var entry in GameState.choicesMadeList)
            {
                var item = new Ink.Runtime.InkListItem("all_directions." + entry);
                list.Add(item, 1); // Arbitrary value
            }

            inkStory.variablesState["choices_made"] = list;

            Debug.Log("Restored choices_made with: " + string.Join(", ", GameState.choicesMadeList));
        }


        // Restore last displayed line
        if (!data.hasStartedDialogue || string.IsNullOrEmpty(data.lastLine))
        {
            // Fresh game start — begin from the top
            ContinueStory();
        }
        else
        {
            // Returning to saved state — show the last line
            currentLine = data.lastLine;
            typingCoroutine = StartCoroutine(TypeText(currentLine));

        }
    }



    public void LoadVisuals()
    {
        // Load visual/audio state
        var p = presentationState;

        if (!string.IsNullOrEmpty(p.lastAmbient)) PlayAmbient(p.lastAmbient);
        if (!string.IsNullOrEmpty(p.lastBackground)) ChangeEnvironmentBackground(p.lastBackground);
        if (!string.IsNullOrEmpty(p.lastSpeaker)) currentSpeaker = p.lastSpeaker;
        if (!string.IsNullOrEmpty(p.lastCharacter) && !string.IsNullOrEmpty(p.lastExpression))
            ChangeCharacterExpression(p.lastCharacter, p.lastExpression);

    }

    public void CheckReturnPoint()
    {
        string returnPoint = SaveSystem.LoadGame().returnPoint;
        
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
            currentLine = inkStory.Continue().Trim();
            DialogueHistoryStatic.AddLine(currentSpeaker ?? "Narrator", currentLine);
            DialogueHistoryStatic.SaveDialogueHistory();

            List<string> tags = inkStory.currentTags;

            HandleTags(tags);
            SaveInkState(); // <-- Still needed here if story continues

            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            typingCoroutine = StartCoroutine(TypeText(currentLine));
        }
        else // End of story
        {
            // Process UIVersion from Ink and update GameState
            ProcessUIVersionFromInk();

            // Clear old Ink state to prepare for new round
            GameState.ResetStoryState();

            // Don't call SaveInkState here — story has ended, so we don’t want to save old ink JSON
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
                    presentationState.lastSpeaker = val;
                    speakerText.text = val;
                    currentSpeaker = val;
                    break;

                case "character":
                    if (val.ToLower() == "off")
                    {
                        presentationState.lastCharacter = "off";
                        currentCharacter = "off";
                        visualManager.HideCharacter();
                    }
                    else
                    {
                        presentationState.lastCharacter = val;
                        currentCharacter = val;
                        visualManager.ShowCharacter(); // Shows character sprite

                    }
                    break;

                case "expression":
                    presentationState.lastExpression = val;
                    ChangeCharacterExpression(currentCharacter, val);
                    break;

                case "background":
                    presentationState.lastBackground = val;
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
            presentationState.lastAmbient = ""; // clear lastAmbient if stopped
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
                presentationState.lastAmbient = clipName; // Track last ambient played
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
        if (inkStory == null) return;

        SaveData data = SaveSystem.LoadGame();
        if (data == null)
            data = new SaveData(); // fallback if no previous save

        data.inkStateJSON = inkStory.state.ToJson();

        // Store presentation data
        data.presentation = presentationState;
        data.lastLine = currentLine;
        // Save Ink variables

        if (inkStory.variablesState["choices_made"] is Ink.Runtime.InkList listVal)
        {
            var savedList = new List<string>();
            foreach (var item in listVal)
            {
                savedList.Add(item.Key.itemName); // e.g., "upDirection", "stay", etc.
            }

            data.choicesMadeList = savedList;
            GameState.choicesMadeList = new List<string>(savedList);
        }

        if (inkStory.variablesState.Contains("trust"))
            data.trust = (int)inkStory.variablesState["trust"];

        if (inkStory.variablesState.Contains("delusion"))
            data.delusion = (int)inkStory.variablesState["delusion"];

        data.hasStartedDialogue = true;

        SaveSystem.SaveGame(data);
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

    public void ProcessUIVersionFromInk()
    {
        if (inkStory == null || inkStory.variablesState["UIVersion"] == null)
        {
            Debug.LogWarning("UIVersion not found in Ink.");
            return;
        }

        int uiVersion = (int)inkStory.variablesState["UIVersion"];

        if (!interfaceVersions.TryGetValue(uiVersion, out InterfaceVersionData versionData))
        {
            Debug.LogWarning($"Unknown UI version: {uiVersion}");
            return;
        }

        // Save return point and version
        SaveData data = SaveSystem.LoadGame() ?? new SaveData();
        data.uiVersion = uiVersion;
        data.returnPoint = versionData.returnPoint;
        SaveSystem.SaveGame(data);

        Debug.Log($"Processed UIVersion {uiVersion} → {versionData.sceneName}");
        GameState.uiVersion = uiVersion;
        GameState.returnPoint = versionData.returnPoint;

        SceneManager.LoadScene(versionData.sceneName);
    }


}