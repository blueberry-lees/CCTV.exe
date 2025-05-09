using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

[RequireComponent(typeof(DialogueChoice))]
public class DialogueManager : MonoBehaviour, IDataPersistence
{
    public int storyProgress  = 0; //test out save game script
    private int deathCount = 0; //test out save game script

    private VisualManager visualManager;
    private DialogueChoice choiceUI;

    public string nextSceneName;

    [Header("UI References")]
    public TMP_Text speakerText;
    public TMP_Text dialogueText;
    

    [Header("Audio")]
    public AudioSource typingAudio;
    public AudioSource sfxAudio;

    [Header("Ink Settings")]
    public TextAsset inkJSON;
    public float defaultSpeed = 0.03f;

    private Story story;
    private bool isTyping = false;
    private bool skipTyping = false;
   

    private string currentSpeaker = "Narrator"; //the current speaker set to narrator by default
    private Dictionary<string, AudioClip> speakerTypingSounds = new();


    private Dictionary<string, string> speakerColors = new()
    {
        { "Male", "#89CFF0" },
        { "Female", "#FFB6C1" },
        { "Narrator", "#FFFFFF" }
    };


    public void LoadData(GameData data)
    {
        this.deathCount = data.deathCount;
        this.storyProgress = data.storyProgress;
        Debug.Log("dialogue data loaded: [" + this.storyProgress + "]");
    }


    public void SaveData(ref GameData data)
    {
        data.deathCount = this.deathCount;
        data.storyProgress = this.storyProgress;
        Debug.Log("dialogue data saved: [" + this.storyProgress + "]");
    }




    void Start()
    {
        visualManager = GetComponent<VisualManager>();

        story = new Story(inkJSON.text);

        // ✅ Set progress into Ink before story runs
        story.variablesState["progress"] = storyProgress;

        // ✅ Build and jump to the appropriate knot
        string knotName = $"chapter_{storyProgress}";
        try
        {
            story.ChoosePathString(knotName);
        }
        catch (StoryException e)
        {
            Debug.LogWarning($"Knot not found: {knotName}. Exception: {e.Message}");
        }

        // ✅ Optional: sync progress back from Ink if it changes immediately
        if (story.variablesState["progress"] is int val)
            storyProgress = val;

        choiceUI = GetComponent<DialogueChoice>();
        choiceUI.Init(story, this);

        LoadTypingSounds();
        ContinueStory();
    }

    void Update()
    {
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

    public void ContinueStory()
    {
        deathCount++;
        //Debug.Log("Death Count: " + deathCount);

        if (!story.canContinue)
        {
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        string line = story.Continue().Trim();
        List<string> tags = story.currentTags;

        // ✅ Update storyProgress after continuing the story
        if (story.variablesState["progress"] is int val)
            storyProgress = val;

        HandleTags(tags);
        StartCoroutine(TypeText(line));
        Debug.Log("Ink progress value: " + story.variablesState["progress"]);
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

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";
        skipTyping = false;

        float speed = GetFloatFromInk("speed", defaultSpeed);
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
        if (story.currentChoices.Count > 0) choiceUI.DisplayChoices();
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
                    speakerText.text = val;
                    currentSpeaker = val;

                    if (val == "Narrator") //if the speaker is narrator close all portrait
                        visualManager.ChangeCharacterExpression("Narrator", ""); // Force hiding portraits
                    break;

                case "sfx":
                    PlaySFX(val);
                    break;

                case "expression":
                    ChangeCharacterExpression(currentSpeaker, val);
                    break;

                case "background":
                    ChangeEnvironmentBackground(val);
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
    private void ChangeCharacterExpression(string speaker, string expression)
    {
        visualManager.ChangeCharacterExpression(speaker, expression);
    }

    private void ChangeEnvironmentBackground(string backgroundName)
    {
        visualManager.ChangeEnvironmentBackground(backgroundName);
    }


}
