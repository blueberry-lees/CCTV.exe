using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState
{
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
        Debug.Log("GameState saved.");
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
        Debug.Log("GameState loaded.");
    }

    public static void ResetAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("GameState reset.");
    }
}
