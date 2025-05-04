using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{

    private VisualManager visualManager;


    [Header("UI References")]
    public TMP_Text speakerText;
    public TMP_Text dialogueText;
    public GameObject choicesContainer;
    public GameObject choicePrefab;
    

    [Header("Choice Colors")]
    public Color selectedChoiceColor = Color.yellow;
    public Color unselectedChoiceColor = Color.white;

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
    private List<TMP_Text> currentChoices = new();
    private int selectedChoiceIndex = 0;

    private Dictionary<string, string> speakerColors = new()
    {
        { "Male", "#89CFF0" },
        { "Female", "#FFB6C1" },
        { "Narrator", "#FFFFFF" }
    };

    void Start()
    {
        visualManager = GetComponent<VisualManager>();
        story = new Story(inkJSON.text);
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

        if (currentChoices.Count > 0)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) SelectNextChoice();
            else if (Input.GetKeyDown(KeyCode.DownArrow)) SelectPreviousChoice();
            else if (Input.GetKeyDown(KeyCode.Return)) ChooseSelectedChoice();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            ContinueStory();
    }

    void ContinueStory()
    {
        ClearChoices();

        if (!story.canContinue)
        {
            PlayerPrefs.SetInt("Round_1_Completed", 1);
            PlayerPrefs.Save();
            SceneManager.LoadScene("Interface2");
            return;
        }

        string line = story.Continue().Trim();        // Get the next line
        List<string> tags = story.currentTags;        // Then grab the tags from this line

        HandleTags(tags);                             // Now handle tags for THIS line
        StartCoroutine(TypeText(line));               // Then show the line
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
        if (story.currentChoices.Count > 0) DisplayChoices();
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

    void DisplayChoices()
    {
        currentChoices.Clear();
        foreach (Choice choice in story.currentChoices)
        {
            var go = Instantiate(choicePrefab, choicesContainer.transform);
            var choiceText = go.GetComponent<TMP_Text>();
            choiceText.text = choice.text;
            currentChoices.Add(choiceText);
        }

        selectedChoiceIndex = 0;
        HighlightChoice();
    }

    void HighlightChoice()
    {
        for (int i = 0; i < currentChoices.Count; i++)
            currentChoices[i].color = i == selectedChoiceIndex ? selectedChoiceColor : unselectedChoiceColor;
    }

    void SelectNextChoice()
    {
        selectedChoiceIndex = (selectedChoiceIndex + 1) % currentChoices.Count;
        HighlightChoice();
    }

    void SelectPreviousChoice()
    {
        selectedChoiceIndex = (selectedChoiceIndex - 1 + currentChoices.Count) % currentChoices.Count;
        HighlightChoice();
    }

    void ChooseSelectedChoice()
    {
        story.ChooseChoiceIndex(selectedChoiceIndex);
        ClearChoices();
        ContinueStory();
    }

    void ClearChoices()
    {
        foreach (Transform child in choicesContainer.transform)
            Destroy(child.gameObject);
        currentChoices.Clear();
    }
}
