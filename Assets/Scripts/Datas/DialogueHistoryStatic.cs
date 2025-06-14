using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public static class DialogueHistoryStatic
{
    /// <summary>
    /// Dialogue history stuff
    /// </summary>
    public static List<string> dialogueLines = new List<string>();

    public static void AddLine(string speaker, string line)
    {
        string timestamp = System.DateTime.Now.ToString("HH:mm:ss");
        dialogueLines.Add($"[{timestamp}] {speaker}: {line}");
    }

    public static string GetDialogueHistoryAsText()
    {
        return string.Join("\n", dialogueLines);
    }

    public static void SaveDialogueHistory()
    {
        string text = GetDialogueHistoryAsText();
        DialogueHistoryFileHandler.Save(text);
    }

    public static void LoadDialogueHistory()
    {
        string loadedText = DialogueHistoryFileHandler.Load();
        dialogueLines = new List<string>(loadedText.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries));
    }

    public static void ClearDialogueHistory()
    {
        dialogueLines.Clear();
        DialogueHistoryFileHandler.Delete();
    }


}


[Serializable]
public struct DialogueLine
{
    public string speaker;
    public string line;
    public string timestamp;

    public DialogueLine(string speaker, string line)
    {
        this.speaker = speaker;
        this.line = line;
        this.timestamp = DateTime.Now.ToString("HH:mm:ss");
    }
}