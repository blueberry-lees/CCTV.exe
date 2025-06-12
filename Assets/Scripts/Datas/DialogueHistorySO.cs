using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class DialogueLine
{
    public string speaker;
    public string line;
    public string timestamp;

    public DialogueLine(string speaker, string line)
    {
        this.speaker = speaker;
        this.line = line;
        this.timestamp = System.DateTime.Now.ToString("HH:mm:ss");
    }
}


[CreateAssetMenu(fileName = "DialogueHistory", menuName = "Game/Dialogue History")]
public class DialogueHistorySO : ScriptableObject
{
    [SerializeField] private List<DialogueLine> dialogueLines = new();

    public void AddLine(string speaker, string line)
    {
        var newLine = new DialogueLine(speaker, line);
        GameState.dialogueHistory.Add(newLine);
        Debug.Log($"[{speaker} @ {System.DateTime.Now:HH:mm:ss}] {line}");
    }

    public void ClearLog()
    {
        GameState.dialogueHistory.Clear();
    }

    public string GetAllLines()
    {
        System.Text.StringBuilder sb = new();
        foreach (var entry in GameState.dialogueHistory)
        {
            sb.AppendLine($"[{entry.timestamp}] {entry.speaker}: {entry.line}");
        }
        return sb.ToString();
    }
}


