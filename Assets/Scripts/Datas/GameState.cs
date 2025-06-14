using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class GameState
{
    public static string inkStateJSON = "";
    public static string returnPoint = "";
    public static string playerName = "";

    public static GamePresentationData presentation = new GamePresentationData();
   
    public static int uiVersion = 1;
    public static int trust = 5; //prob dont need this since is saved through ink BUT I'm not sure if having muiltiple return point will affect that so lets check this later
    public static int delusion = 5;//prob dont need this since is saved through ink BUT I'm not sure if having muiltiple return point will affect that so lets check this later

    

    public static void SaveAll()
    {
        SaveData data = new SaveData()
        {
            //strings
            inkStateJSON = inkStateJSON,
            playerName = playerName,
            returnPoint = returnPoint,

            //class
            presentation = presentation,

            //ints
            uiVersion = uiVersion,
            trust = trust,
            delusion = delusion,

        };

        SaveSystem.SaveGame(data);
    }

    public static void LoadAll()
    {
        SaveData data = SaveSystem.LoadGame();
        if (data == null) return;

        //strings
        inkStateJSON = data.inkStateJSON;
        returnPoint = data.returnPoint;
        playerName = data.playerName;

        //class
        presentation = data.presentation ?? new GamePresentationData(); // fallback just in case

        //ints
        uiVersion = data.uiVersion;
        trust = data.trust;
        delusion = data.delusion;
       
    }

    public static void ResetAll()
    {
        SaveSystem.DeleteSave();
        Debug.Log("GameState reset.");
    }
}


[System.Serializable]
public class GamePresentationData
{
    public string lastBackground = "";
    public string lastCharacter = "";
    public string lastExpression = "";
    public string lastSpeaker = "";
    public string lastAmbient = "";
}