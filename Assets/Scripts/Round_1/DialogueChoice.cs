using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;

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

    private List<TMP_Text> currentChoices = new();
    private int selectedChoiceIndex = 0;
    private Story story;
    public bool HasChoices() => currentChoices.Count > 0;

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
            var choiceText = Instantiate(choicePrefab, choicesContainer.transform)
                             .GetComponent<TMP_Text>();
            choiceText.text = choice.text;
            currentChoices.Add(choiceText);
        }

        selectedChoiceIndex = 0;
        UpdateChoiceColors();
    }

    public void NavigateChoice(int direction)
    {
        selectedChoiceIndex = (selectedChoiceIndex + direction + currentChoices.Count) % currentChoices.Count;
        UpdateChoiceColors();
        PlaySound(navigateClip);
    }

    public void ChooseSelectedChoice()
    {
        story.ChooseChoiceIndex(selectedChoiceIndex);
        ClearChoices();
        dialogueManager.ContinueStory();
    }

    private void UpdateChoiceColors()
    {
        for (int i = 0; i < currentChoices.Count; i++)
            currentChoices[i].color = i == selectedChoiceIndex ? selectedChoiceColor : unselectedChoiceColor;
    }

    public void ClearChoices()
    {
        foreach (Transform child in choicesContainer.transform)
            Destroy(child.gameObject);
        currentChoices.Clear();
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
            audioSource.PlayOneShot(clip);
    }
}
