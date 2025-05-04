using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text dialogueText;
    public TMP_Text speakerText;
    public GameObject choicesContainer;
    public GameObject choicePrefab;

    [Header("Portraits")]
    public Image femalePortraitImage;
    public Image malePortraitImage;

    [Header("Background")]
    public Image backgroundImage;

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

    private string currentSpeaker = "Narrator";
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
        femalePortraitImage.gameObject.SetActive(false);
        malePortraitImage.gameObject.SetActive(false);

        story = new Story(inkJSON.text);
        LoadTypingSounds();
        ContinueStory();
    }



    void LoadTypingSounds()
    {
        foreach (string type in new[] { "Male", "Female", "Narrator" })
        {
            var clip = Resources.Load<AudioClip>($"Audio/Typing/{type}");
            if (clip) speakerTypingSounds[type.ToLower()] = clip;
        }
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

    void PlaySFX(string clipName)
    {
        var clip = Resources.Load<AudioClip>($"Audio/SFX/{clipName}");
        if (clip) sfxAudio.PlayOneShot(clip);
        else Debug.LogWarning($"SFX not found: {clipName}");
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

                case "effect":
                    ApplyTextEffect(val);
                    break;

                case "background":
                    ChangeEnvironmentBackground(val);
                    break;
            }
        }
    }

    private void ChangeCharacterExpression(string speaker, string expression)
    {
        string path = $"Portraits/{speaker}/{expression}";
        Sprite portrait = Resources.Load<Sprite>(path);
        if (!portrait)
        {
            Debug.LogWarning($"Portrait not found: {path}");
            return;
        }

        // Hide both portraits before switching
        femalePortraitImage.gameObject.SetActive(false);
        malePortraitImage.gameObject.SetActive(false);

        switch (speaker)
        {
            case "Female":
                femalePortraitImage.sprite = portrait;
                femalePortraitImage.gameObject.SetActive(true);
                break;

            case "Male":
                malePortraitImage.sprite = portrait;
                malePortraitImage.gameObject.SetActive(true);
                break;

            default:
                Debug.Log($"Unknown speaker: {speaker}");
                break;

                //can add more speaker/character
        }
    }

    void ApplyTextEffect(string effect)
    {
        // Future effect handling (e.g. shake/wave)
        Debug.Log($"Apply Effect: {effect}");
    }

    void ChangeEnvironmentBackground(string backgroundName)
    {
        Sprite bgSprite = Resources.Load<Sprite>($"Backgrounds/{backgroundName}");
        if (bgSprite != null)
        {
            backgroundImage.sprite = bgSprite;
            backgroundImage.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"Background not found: {backgroundName}");
        }
    }
}
