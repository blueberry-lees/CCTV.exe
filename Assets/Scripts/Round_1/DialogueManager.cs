using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;


public enum Chapters { Round1, Round2, Round3, Round4 }

[RequireComponent(typeof(DialogueChoice))]
public class DialogueManager : MonoBehaviour, IDataPersistence
{
    [Header("Data to save")]
    public int storyProgress  = 0; //test out save game script
    public Chapters currentChapter = Chapters.Round1;


    [Header("Scripts ref")]
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
    //public TextAsset inkJSON;
    public TextAsset currentInkJSON;
    public TextAsset r1, r2, r3, r4;


    public float defaultSpeed = 0.03f;

    private Story story;
    private bool isTyping = false;
    private bool skipTyping = false;

    private string currentCharacter = "Narrator";
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
        this.storyProgress = data.storyProgress;
        Debug.Log("dialogue data loaded: [" + this.storyProgress + "]");

    }


    public void SaveData(ref GameData data)
    {
        data.storyProgress = this.storyProgress;
        Debug.Log("dialogue data saved: [" + this.storyProgress + "]");

        
    }


    //choose the chapter (enum) to chage the inkJson files accordingly
    public void DefineChapter(Chapters selectedChapter)
    {
        currentChapter = selectedChapter;

        switch (selectedChapter)
        {
            case Chapters.Round1: currentInkJSON = r1;  break;
            case Chapters.Round2: currentInkJSON = r2;  break;
            case Chapters.Round3: currentInkJSON = r3; break;
            case Chapters.Round4: currentInkJSON = r4; break;

        }
        
    }


    

    void Start()
    {
        DefineChapter(Chapters.Round1);


        visualManager = GetComponent<VisualManager>();

        story = new Story(currentInkJSON.text);

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
        //TODO: how to save dialogue progress without the story progress int?

        if (!story.canContinue)
        {
            SaveStoryRound(1);
            Debug.Log("no more story/story ends");
            //TODO: playerpref round 1 end...
            //if round 1 end go to interface version 2
            //from version 2 set story progress to 7, and load round 1 scene again with round 2 contents

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
                    currentSpeaker = val; // This controls text color and typing sound
                    break;

                case "character":
                    currentCharacter = val; // This controls which visual asset is used
                    break;

                case "expression":
                    ChangeCharacterExpression(currentCharacter, val); // Use currentCharacter here
                    break;

                case "background":
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





    public void SaveStoryRound(int completedRound)
    {
        PlayerPrefs.SetInt("Round", completedRound);
        PlayerPrefs.Save();
    }

}

