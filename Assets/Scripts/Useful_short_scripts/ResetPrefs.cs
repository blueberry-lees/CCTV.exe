using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPrefs : MonoBehaviour
{
    void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs reset.");
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey("InkState");
        PlayerPrefs.DeleteKey("Round");
        PlayerPrefs.Save();
    }
    
}
