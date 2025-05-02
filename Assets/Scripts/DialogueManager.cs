using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;


public class DialogueManager : MonoBehaviour
{
    public UnityEngine.UI.Image portraitImage; // Assign in Inspector
    private string currentSpeaker = "Narrator"; // Default

    public TextAsset inkJSON;
    public TMP_Text dialogueText;
    public TMP_Text speakerText;
    public GameObject choicesContainer;
    public GameObject choicePrefab;
    public AudioSource typingAudio;
    public float defaultSpeed = 0.03f;

    private Story story;
    private bool isTyping = false;
    private bool skipTyping = false;
    private List<TMP_Text> currentChoices = new List<TMP_Text>();
    private int selectedChoiceIndex = 0;

    void Start()
    {
        story = new Story(inkJSON.text);
        ContinueStory();
    }

    void Update()
    {

        // If choices are visible, handle them first
        if (currentChoices.Count > 0)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
                SelectNextChoice();
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                SelectPreviousChoice();
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                ChooseSelectedChoice();
                return; // Stop further input this frame
            }
        }

        // Handle dialogue skipping or continuing
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping)
            {
                skipTyping = true;
            }
            else
            {
                ContinueStory();
            }
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

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";
        skipTyping = false;

        float speed = GetFloatFromInk("speed", defaultSpeed);
        float delay = GetFloatFromInk("delay", 0f);

        yield return new WaitForSeconds(delay);

        foreach (char c in text)
        {
            if (skipTyping)
            {
                dialogueText.text = text;
                break;
            }
            dialogueText.text += c;
            if (typingAudio != null)
                typingAudio.Play();
            yield return new WaitForSeconds(speed);
        }

        isTyping = false;

        if (story.currentChoices.Count > 0)
            DisplayChoices();
    }

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
            currentChoices[i].color = (i == selectedChoiceIndex) ? Color.yellow : Color.white;
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
        Debug.Log("the end here");
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

    void HandleTags(List<string> tags)
    {
        foreach (string tag in tags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2) continue;

            string key = splitTag[0].Trim().ToLower();
            string val = splitTag[1].Trim();

            if (key == "speaker")
            {
                speakerText.text = val;
                currentSpeaker = val;
            }
            else if (key == "expression")
            {
                ChangeCharacterExpression(currentSpeaker, val);
            }
            else if (key == "effect")
            {
                ApplyTextEffect(val);
            }
        }
    }
    void ChangeCharacterExpression(string speaker, string expression)
    {
        string spritePath = $"Portraits/{speaker}_{expression}";
        Sprite newPortrait = Resources.Load<Sprite>(spritePath);

        if (newPortrait != null)
        {
            portraitImage.sprite = newPortrait;
        }
        else
        {
            Debug.LogWarning($"Portrait not found: {spritePath}");
        }
    }

    

    void ApplyTextEffect(string effect)
    {
        // Example: shake, wave, fade (TMP rich text or shader)
        Debug.Log("Apply Effect: " + effect);
    }
}

