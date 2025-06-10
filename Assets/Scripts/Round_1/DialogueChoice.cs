using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;

public class DialogueChoice : MonoBehaviour
{
    private DialogueManager dialogueManager;

    [Header("Choice Panel")]
    public Color selectedChoiceColor = Color.yellow;
    public Color unselectedChoiceColor = Color.white;
    public GameObject choicesContainer;
    public GameObject choicePrefab;

    [Header("Audio")]
    public AudioClip navigateClip;

    private AudioSource audioSource;

    private List<Button> choiceButtons = new();
    private int selectedChoiceIndex = 0;
    private Story story;
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
        this.story = story;
        this.dialogueManager = manager;
    }

    public void DisplayChoices()
    {
        if (story == null)
        {
            Debug.LogError("DialogueChoice: Story is null. Did you forget to call Init()?");
            return;
        }

        ClearChoices();

        foreach (Choice choice in story.currentChoices)
        {
            GameObject choiceGO = Instantiate(choicePrefab, choicesContainer.transform);
            Button button = choiceGO.GetComponent<Button>();
            TMP_Text choiceText = choiceGO.GetComponent<TMP_Text>(); // Same object

            if (button == null || choiceText == null)
            {
                Debug.LogError("Choice prefab must have both a Button and TMP_Text on the same GameObject.");
                continue;
            }

            choiceText.text = choice.text;
            int choiceIndex = choice.index;
            button.onClick.AddListener(() => {
                if (choicesAreInteractable)
                    SelectChoice(choiceIndex);
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

        int choiceIndex = story.currentChoices[selectedChoiceIndex].index;
        SelectChoice(choiceIndex);
    }

    private void SelectChoice(int choiceIndex)
    {
        story.ChooseChoiceIndex(choiceIndex);
        ClearChoices();
        dialogueManager.ContinueStory();
    }

    private void UpdateChoiceVisuals()
    {
        for (int i = 0; i < choiceButtons.Count; i++)
        {
            TMP_Text text = choiceButtons[i].GetComponent<TMP_Text>(); // Same object
            if (text != null)
                text.color = (i == selectedChoiceIndex) ? selectedChoiceColor : unselectedChoiceColor;
        }
    }

    public void SetChoicesInteractable(bool state)
    {
        choicesAreInteractable = state;

        // Save whether choices were up when disabling them
        if (!state && HasChoices())
            shouldRestoreChoices = true;

        foreach (var button in choiceButtons)
            button.interactable = state;
    }

    public void RestoreChoicesIfNeeded()
    {
        if (shouldRestoreChoices && story != null && story.currentChoices.Count > 0)
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
