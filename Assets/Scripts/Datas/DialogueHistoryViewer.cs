using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueHistoryViewer : MonoBehaviour
{
    [SerializeField] private TMP_Text historyText;

    private void Awake()
    {
        historyText = GetComponent<TMP_Text>();

        // Load saved history from file
        DialogueHistoryStatic.LoadDialogueHistory();
        ShowHistory();
    }

    private void OnEnable()
    {
        DialogueHistoryStatic.LoadDialogueHistory(); // Re-load in case it changed elsewhere
        ShowHistory();
    }

    public void ShowHistory()
    {
        if (historyText != null)
            historyText.text = DialogueHistoryStatic.GetDialogueHistoryAsText();
    }

    public void ClearHistory()
    {
        DialogueHistoryStatic.ClearDialogueHistory();
        DialogueHistoryStatic.SaveDialogueHistory(); // Save the cleared state

        if (historyText != null)
            historyText.text = "";
    }


}
