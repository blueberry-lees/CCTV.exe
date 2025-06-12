using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueHistoryViewer : MonoBehaviour
{

    [SerializeField] private DialogueHistorySO dialogueHistory;
    [HideInInspector][SerializeField] private TMP_Text historyText;


    private void Awake()
    {
        historyText = GetComponent<TMP_Text>();
    }
    private void OnEnable()
    {
       ShowHistory();
    }

    public void ShowHistory()
    {
        historyText.text = dialogueHistory.GetLog();
    }

    public void ClearHistory()
    {
        dialogueHistory.ClearLog();
        historyText.text = "";
    }

    private void OnDisable()
    {
        ClearHistory();
    }
}
