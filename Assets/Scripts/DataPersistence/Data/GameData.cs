using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public enum Rounds { Round1, Round2, Round3, Round4 }
    public Rounds currentRound = Rounds.Round1;
    public string savedInkStateJSON;
    public int storyProgress;



    //the values defined in this contructor will be the default values 
    //the game starts with when there's no data to load
    public GameData()
    {
      this.currentRound = Rounds.Round1;
        this.storyProgress = 0;
        
    }
}
