using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState
{
    public static string inkStateJSON = "";

    public static List<DialogueLine> dialogueHistory = new();


    [Header("Last tags in ink")]
    public static string lastBackground = "";
    public static string lastCharacter = "";
    public static string lastExpression = "";
    public static string lastSpeaker = "";

    [Header("Where the next knot to go when enter story?")]
    public static string returnPoint = "";

    [Header("What version should the Interface be?")]
    public static int uiVersion = 1;



    public static void SaveAll()
    {
        PlayerPrefs.SetString("InkState", inkStateJSON);

        PlayerPrefs.SetString("LastBackground", lastBackground);
        PlayerPrefs.SetString("LastCharacter", lastCharacter);
        PlayerPrefs.SetString("LastExpression", lastExpression);
        PlayerPrefs.SetString("LastSpeaker", lastSpeaker);


        PlayerPrefs.SetString("ReturnPoint", returnPoint);

        PlayerPrefs.SetInt("UIVersion", uiVersion);

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

    public static string GetDialogueHistoryAsText()
    {
        Debug.Log("Dialogue history count: " + dialogueHistory.Count); // <== test this

        System.Text.StringBuilder sb = new();
        foreach (var entry in dialogueHistory)
        {
            sb.AppendLine($"[{entry.timestamp}] {entry.speaker}: {entry.line}");
        }
        Debug.Log("what about here: " + dialogueHistory.Count); // <== test this
        return sb.ToString();
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
