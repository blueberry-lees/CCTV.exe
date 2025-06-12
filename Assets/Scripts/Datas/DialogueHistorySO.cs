using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DialogueHistory", menuName = "Game/Dialogue History")]
public class DialogueHistorySO : ScriptableObject
{
    [TextArea(3, 20)]
    public List<string> dialogueLog = new();

    public void AddLine(string line)
    {
        dialogueLog.Add(line);
    }

    public void ClearLog()
    {
        dialogueLog.Clear();
    }

    public string GetLog()
    {
        return string.Join("\n", dialogueLog);
    }
}
