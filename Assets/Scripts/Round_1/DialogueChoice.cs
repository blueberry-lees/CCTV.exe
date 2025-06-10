using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;

public class DialogueChoice : MonoBehaviour
{
    public Button choiceButtonPrefab;    // assign your prefab in inspector
    public Transform choicesContainer;   // parent panel for buttons

    private Story story;
    private DialogueManager dialogueManager;

    private List<Button> choiceButtons = new();

    private int selectedChoiceIndex = 0;

    void Awake()
    {
        dialogueManager = GetComponent<DialogueManager>();
    }

    public void Init(Story storyRef, DialogueManager manager)
    {
        story = storyRef;
        dialogueManager = manager;
    }

    public bool HasChoices()
    {
        return story != null && story.currentChoices.Count > 0;
    }

    public void DisplayChoices()
    {
        ClearChoices();

        List<Choice> choices = story.currentChoices;

        for (int i = 0; i < choices.Count; i++)
        {
            int index = i;  // capture for closure
            Button btn = Instantiate(choiceButtonPrefab, choicesContainer);
            btn.gameObject.SetActive(true);

            TMP_Text btnText = btn.GetComponentInChildren<TMP_Text>();
            if (btnText != null)
                btnText.text = choices[i].text.Trim();

            btn.onClick.AddListener(() => OnChoiceSelected(index));

            choiceButtons.Add(btn);
        }

        selectedChoiceIndex = 0;
        UpdateButtonHighlight();
    }

    public void HideChoices()
    {
        ClearChoices();
    }

    private void ClearChoices()
    {
        foreach (var btn in choiceButtons)
        {
            btn.onClick.RemoveAllListeners();
            Destroy(btn.gameObject);
        }
        choiceButtons.Clear();
    }

    private void OnChoiceSelected(int index)
    {
        if (story == null) return;

        story.ChooseChoiceIndex(index);
        HideChoices();
        dialogueManager.ContinueStory();
    }

    // Keyboard navigation
    public void NavigateChoice(int direction)
    {
        if (choiceButtons.Count == 0) return;

        selectedChoiceIndex += direction;

        if (selectedChoiceIndex < 0) selectedChoiceIndex = choiceButtons.Count - 1;
        else if (selectedChoiceIndex >= choiceButtons.Count) selectedChoiceIndex = 0;

        UpdateButtonHighlight();
    }

    private void UpdateButtonHighlight()
    {
        for (int i = 0; i < choiceButtons.Count; i++)
        {
            var colors = choiceButtons[i].colors;
            if (i == selectedChoiceIndex)
            {
                colors.normalColor = Color.yellow; // highlight color
                choiceButtons[i].Select();          // Unity's UI system highlight
            }
            else
            {
                colors.normalColor = Color.white;  // default color
            }
            choiceButtons[i].colors = colors;
        }
    }

    public void ChooseSelectedChoice()
    {
        if (selectedChoiceIndex >= 0 && selectedChoiceIndex < choiceButtons.Count)
        {
            OnChoiceSelected(selectedChoiceIndex);
        }
    }
}
