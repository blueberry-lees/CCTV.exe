using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text dialogueText;
    public TMP_Text speakerText;
    public Image portraitImage;
    public GameObject choicesContainer;
    public GameObject choicePrefab;

    [Header("Choice Colors")]
    public Color selectedChoiceColor = Color.yellow;
    public Color unselectedChoiceColor = Color.white;

    [Header("Audio")]
    public AudioSource typingAudio;
    public AudioSource sfxAudio;
    public TextAsset inkJSON;

    [Header("Typing Settings")]
    public float defaultSpeed = 0.03f;

    private Story story;
    private bool isTyping = false;
    private bool skipTyping = false;

    private string currentSpeaker = "Narrator";
    private Dictionary<string, AudioClip> speakerTypingSounds = new();
    private List<TMP_Text> currentChoices = new();
    private int selectedChoiceIndex = 0;

    //define text color based on the speaker
    private Dictionary<string, string> speakerColors = new Dictionary<string, string>()
{
    { "Male", "#89CFF0" },     // light blue
    { "Female", "#FFB6C1" },   // light pink
    { "Narrator", "#FFFFFF" }  // white (default)
};


    // ───────────────────────────── Init ─────────────────────────────
    void Start()
    {
        story = new Story(inkJSON.text);
        LoadTypingSounds();
        ContinueStory();
    }

    void LoadTypingSounds()
    {
        string[] speakerTypes = { "Male", "Female", "Narrator" };

        foreach (string type in speakerTypes)
        {
            var clip = Resources.Load<AudioClip>($"Audio/Typing/{type}");
            if (clip != null)
                speakerTypingSounds[type.ToLower()] = clip;
        }
    }

    // ───────────────────────────── Update ─────────────────────────────
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

    // ───────────────────────────── Story Flow ─────────────────────────────
    void ContinueStory()
    {
        ClearChoices();

        if (story.canContinue)
        {
            string text = story.Continue().Trim();
            HandleTags(story.currentTags);
            StartCoroutine(TypeText(text));
        }
        else
        {
            Debug.Log("End of story.");
        }
    }

    float GetFloatFromInk(string varName, float defaultValue)
    {
        var val = story.variablesState[varName];
        if (val is int i) return i;
        if (val is float f) return f;
        if (val is double d) return (float)d;
        return defaultValue;
    }

    // ───────────────────────────── Typing ─────────────────────────────
    IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";
        skipTyping = false;

        float speed = GetFloatFromInk("speed", defaultSpeed);
        float delay = GetFloatFromInk("delay", 0f);
        yield return new WaitForSeconds(delay);

        // Wrap text in color tag
        string colorHex = speakerColors.ContainsKey(currentSpeaker) ? speakerColors[currentSpeaker] : "#FFFFFF";
        string colorStart = $"<color={colorHex}>";
        string colorEnd = "</color>";

        string fullText = colorStart + text + colorEnd;
        int i = 0;

        while (i < fullText.Length)
        {
            if (skipTyping)
            {
                dialogueText.text = fullText;
                break;
            }

            // Handle TMP tags immediately (like <color=#xxxxxx>)
            if (fullText[i] == '<')
            {
                int closingIndex = fullText.IndexOf('>', i);
                if (closingIndex != -1)
                {
                    string tag = fullText.Substring(i, closingIndex - i + 1);
                    dialogueText.text += tag;
                    i = closingIndex + 1;
                    continue;
                }
            }

            dialogueText.text += fullText[i];
            i++;

            if (typingAudio != null && speakerTypingSounds.TryGetValue(currentSpeaker.ToLower(), out AudioClip clip))
            {
                typingAudio.clip = clip;
                typingAudio.Play();
            }

            yield return new WaitForSeconds(speed);
        }

        isTyping = false;

        if (story.currentChoices.Count > 0)
            DisplayChoices();
    }



    // ───────────────────────────── SFX ─────────────────────────────

    void PlaySFX(string clipName)
    {
        AudioClip clip = Resources.Load<AudioClip>($"Audio/SFX/{clipName}");
        if (clip != null)
        {
            sfxAudio.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"SFX not found: Audio/SFX/{clipName}");
        }
    }


    // ───────────────────────────── Choices ─────────────────────────────
    void DisplayChoices()
    {
        currentChoices.Clear();

        foreach (Choice choice in story.currentChoices)
        {
            GameObject choiceGO = Instantiate(choicePrefab, choicesContainer.transform);
            TMP_Text choiceText = choiceGO.GetComponent<TMP_Text>();
            choiceText.text = choice.text;
            currentChoices.Add(choiceText);
        }

        selectedChoiceIndex = 0;
        HighlightChoice();
    }

    void HighlightChoice()
    {
        for (int i = 0; i < currentChoices.Count; i++)
        {
            currentChoices[i].color = (i == selectedChoiceIndex) ? selectedChoiceColor : unselectedChoiceColor;
        }
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

    // ───────────────────────────── Tags ─────────────────────────────
    void HandleTags(List<string> tags)
    {
        foreach (string tag in tags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2) continue;

            string key = splitTag[0].Trim().ToLower();
            string val = splitTag[1].Trim();

            switch (key)
            {
                case "speaker": //speaker of the dialogue
                    speakerText.text = val;
                    currentSpeaker = val;
                    break;
                case "expression"://character expression in dialogue
                    ChangeCharacterExpression(currentSpeaker, val);
                    break;
                case "effect": //text effect in dialogue
                    ApplyTextEffect(val);
                    break;
                case "sfx": //sound effect in dialogue
                    PlaySFX(val); 
                    break;
            }
        }
    }

    void ChangeCharacterExpression(string speaker, string expression)
    {
        string spritePath = $"Portraits/{speaker}_{expression}";
        Sprite newPortrait = Resources.Load<Sprite>(spritePath);

        if (newPortrait != null)
            portraitImage.sprite = newPortrait;
        else
            Debug.LogWarning($"Portrait not found: {spritePath}");
    }

    void ApplyTextEffect(string effect)
    {
        // Placeholder for visual text effects (e.g. wave, shake)
        Debug.Log($"Apply Effect: {effect}");
    }
}
