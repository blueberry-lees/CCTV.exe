using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNameHandler :MonoBehaviour
{
    public static PlayerNameHandler instance;

    public string playerName { get; private set; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist this object across scenes
            playerName = PlayerPrefs.GetString("playerName", "");
        }
        else
        {
            Destroy(gameObject);
        }
      

    }

    public void SavePlayerName(string name)
    {
        playerName = name;
        PlayerPrefs.SetString("playerName", name);
        PlayerPrefs.SetInt("hasPlayedBefore", 1);
        PlayerPrefs.Save();
    }

    public bool HasSavedName()
    {
        return !string.IsNullOrEmpty(playerName);
    }


    public void ResetPlayerPref()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.LogWarning("PlayerPrefs have been reset.");
    }
}
