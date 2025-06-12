using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueHistoryViewer : MonoBehaviour
{

    [SerializeField] private DialogueHistorySO dialogueHistory;
    [SerializeField] private TMP_Text historyText;


    private void Awake()
    {
        historyText = GetComponent<TMP_Text>();
        ShowHistory();
    }
    private void OnEnable()
    {
       ShowHistory();
    }

    public void ShowHistory()
    {

        historyText.text = GameState.GetDialogueHistoryAsText();
    }

    public void ClearHistory()
    {
        dialogueHistory.ClearLog();
        historyText.text = "";
    }

  
}
