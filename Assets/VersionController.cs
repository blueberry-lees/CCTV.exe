using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum Rounds { Round1, Round2, Round3, Round4 }

public class VersionController 
{
    //public Rounds currentRound = Rounds.Round1;






    public void SaveStoryRound(int completedRound)
    {
        PlayerPrefs.SetInt("Round", completedRound);
        PlayerPrefs.Save();
    }
   


}
