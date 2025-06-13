using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class GameState
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






    /// <summary>
    /// Player pref stuffs
    /// </summary>
    /// 
    public static string inkStateJSON = "";

    [Header("Last tags in ink")]
    public static string lastBackground = "";
    public static string lastCharacter = "";
    public static string lastExpression = "";
    public static string lastSpeaker = "";

    [Header("Where the next knot to go when enter story?")]
    public static string returnPoint = "";

    [Header("What version should the Interface be?")]
    public static int uiVersion = 1;
    //public static int fileConfirmed = 0;

    public static int trust;
    public static int delusion;


    public static void SaveAll()
    {
       

            PlayerPrefs.SetString("InkState", inkStateJSON);

        PlayerPrefs.SetString("LastBackground", lastBackground);
        PlayerPrefs.SetString("LastCharacter", lastCharacter);
        PlayerPrefs.SetString("LastExpression", lastExpression);
        PlayerPrefs.SetString("LastSpeaker", lastSpeaker);


        PlayerPrefs.SetString("ReturnPoint", returnPoint);

        PlayerPrefs.SetInt("UIVersion", uiVersion);

        //PlayerPrefs.SetInt("fileConfirmed", 0);
        PlayerPrefs.SetInt("Trust", trust);
        PlayerPrefs.SetInt("Delusion", delusion);

        PlayerPrefs.Save();
        //Debug.Log("GameState saved.");
    }

    public static void LoadAll()
    {
        inkStateJSON = PlayerPrefs.GetString("InkState", "");
        lastBackground = PlayerPrefs.GetString("LastBackground", "");
        lastCharacter = PlayerPrefs.GetString("LastCharacter", "");
        lastExpression = PlayerPrefs.GetString("LastExpression", "");
        lastSpeaker = PlayerPrefs.GetString("LastSpeaker", "");
        returnPoint = PlayerPrefs.GetString("ReturnPoint", "");
        uiVersion = PlayerPrefs.GetInt("UIVersion", 1);

        //fileConfirmed = PlayerPrefs.GetInt("fileConfirmed", 0);
        trust = PlayerPrefs.GetInt("Trust", 5);      // fallback to INITIAL_SWING
        delusion = PlayerPrefs.GetInt("Delusion", 5);
        //Debug.Log("GameState loaded.");
    }

    public static void ResetAll()
    {
        PlayerPrefs.DeleteAll();
        //dialogueHistory.ClearLog();
        PlayerPrefs.Save();
        Debug.Log("GameState reset.");
    }


    [Tooltip("This is just to remove the previous tags/sprites used")]
    public static void ResetStoryData()
    {
        PlayerPrefs.SetString("InkState", "");
        PlayerPrefs.SetString("LastBackground", "");
        PlayerPrefs.SetString("LastCharacter", "");
        PlayerPrefs.SetString("LastExpression", "");
        PlayerPrefs.SetString("LastSpeaker", "");
    }

    public static void SaveVersion()
    {
        PlayerPrefs.SetString("ReturnPoint", returnPoint);

        PlayerPrefs.SetInt("UIVersion", uiVersion);

        PlayerPrefs.Save();
    }


    [Tooltip("This is just to remove the previous tags/sprites used")]
    public static void ResetStoryData2()
    {

        PlayerPrefs.DeleteKey("InkState");
        PlayerPrefs.DeleteKey("LastBackground");
        PlayerPrefs.DeleteKey("LastCharacter");
        PlayerPrefs.DeleteKey("LastExpression");
        PlayerPrefs.DeleteKey("LastSpeaker");
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