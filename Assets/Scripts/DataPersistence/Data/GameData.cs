using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int deathCount;
    public int storyProgress;
    public float totalPlayTimeSeconds; // total play time in seconds


    //the values defined in this contructor will be the default values 
    //the game starts with when there's no data to load
    public GameData()
    {
        this.deathCount = 0;
        this.storyProgress = 0;
        this.totalPlayTimeSeconds = 0f;
    }

    public int GetTimePlayedMinutes()
    {
        return Mathf.FloorToInt(totalPlayTimeSeconds / 60f); // return minutes
    }

    public string GetFormattedPlayTime()
    {
        int hours = Mathf.FloorToInt(totalPlayTimeSeconds / 3600f);
        int minutes = Mathf.FloorToInt((totalPlayTimeSeconds % 3600f) / 60f);
        int seconds = Mathf.FloorToInt(totalPlayTimeSeconds % 60f);
        return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    }
}
