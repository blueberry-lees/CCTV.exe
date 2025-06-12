using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;

public class DialogueChoice : MonoBehaviour
{
    private DialogueManager dialogueManager;


    [Header("Choice Panel")]
    public Color selectedChoiceColor = Color.black;
    public Color unselectedChoiceColor = Color.white;
    public Color selectedBackgroundColor = Color.white;
    public Color unselectedBackgroundColor = Color.black;
    public GameObject choicesContainer;
    public GameObject choicePrefab;

    [Header("Audio")]
    public AudioClip navigateClip;

    private AudioSource audioSource;

    private List<Button> choiceButtons = new();
    private int selectedChoiceIndex = 0;
    private Story _inkStory;
    private bool choicesAreInteractable = true;
    private bool shouldRestoreChoices = false;

    public bool HasChoices() => choiceButtons.Count > 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
            Debug.LogWarning("DialogueChoice: No AudioSource found on this GameObject.");
    }

    public void Init(Story story, DialogueManager manager)
    {
        this._inkStory = story;
        this.dialogueManager = manager;
    }

    public void DisplayChoices()
    {
        if (_inkStory == null)
        {
            Debug.LogError("DialogueChoice: Story is null. Did you forget to call Init()?");
            return;
        }

        ClearChoices();

        foreach (Choice choice in _inkStory.currentChoices)
        {
            GameObject choiceGO = Instantiate(choicePrefab, choicesContainer.transform);
            Button button = choiceGO.GetComponent<Button>();
            TMP_Text choiceText = choiceGO.GetComponentInChildren<TMP_Text>(); // prefab must have text conponent in children

            if (button == null || choiceText == null)
            {
                Debug.LogError("Choice prefab must have a Button on parent and TMP_Text on child.");
                continue;
            }

            choiceText.text = choice.text;
            string displayedChoiceText = choice.text; // capture to avoid closure issue for dialogue history
            int choiceIndex = choice.index;

            button.onClick.AddListener(() => {
                if (choicesAreInteractable)
                {
                    Debug.Log("Logging choice: " + displayedChoiceText); // Debug here
                    GameState.AddLine("You", displayedChoiceText);
                    GameState.SaveDialogueHistory();  // "You" is the #speaker in ink  // Log the chosen text when mouse clicked on choice
                    SelectChoice(choiceIndex);
                }
                    
            });

            choiceButtons.Add(button);
        }

        selectedChoiceIndex = 0;
        UpdateChoiceVisuals();
    }

    public void NavigateChoice(int direction)
    {
        if (!choicesAreInteractable || choiceButtons.Count == 0) return;

        selectedChoiceIndex = (selectedChoiceIndex + direction + choiceButtons.Count) % choiceButtons.Count;
        UpdateChoiceVisuals();
        PlaySound(navigateClip);
    }

    public void ChooseSelectedChoice()
    {
        if (!choicesAreInteractable || choiceButtons.Count == 0) return;

        int choiceIndex = _inkStory.currentChoices[selectedChoiceIndex].index;

        string choiceText = _inkStory.currentChoices[selectedChoiceIndex].text;

        GameState.AddLine("You", choiceText);
        GameState.SaveDialogueHistory(); // keybaord input to log history

        SelectChoice(choiceIndex);
    }

    private void SelectChoice(int choiceIndex)
    {
        _inkStory.ChooseChoiceIndex(choiceIndex);
        ClearChoices();
        dialogueManager.ContinueStory();
    }

    private void UpdateChoiceVisuals()
    {
        for (int i = 0; i < choiceButtons.Count; i++)
        {
            Button button = choiceButtons[i];
            TMP_Text text = button.GetComponentInChildren<TMP_Text>();
            Image image = button.GetComponent<Image>();

            bool isSelected = i == selectedChoiceIndex;

            if (text != null)
                text.color = isSelected ? selectedChoiceColor : unselectedChoiceColor;

            if (image != null)
                image.color = isSelected ? selectedBackgroundColor : unselectedBackgroundColor;
        }
    }

    public void SetChoicesInteractable(bool state)
    {
        choicesAreInteractable = state;

        if (!state && HasChoices())
            shouldRestoreChoices = true;

        foreach (var button in choiceButtons)
            button.interactable = state;
    }

    public void RestoreChoicesIfNeeded()
    {
        if (shouldRestoreChoices && _inkStory != null && _inkStory.currentChoices.Count > 0)
        {
            DisplayChoices();
        }

        shouldRestoreChoices = false;
    }

    public void ClearChoices()
    {
        foreach (Transform child in choicesContainer.transform)
            Destroy(child.gameObject);
        choiceButtons.Clear();
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
            audioSource.PlayOneShot(clip);
    }
}
