using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNameHandler : MonoBehaviour
{
    public static PlayerNameHandler Instance;

    public string PlayerName { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist this object across scenes
            PlayerName = PlayerPrefs.GetString("playerName", "");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SavePlayerName(string name)
    {
        PlayerName = name;
        PlayerPrefs.SetString("playerName", name);
        PlayerPrefs.SetInt("hasPlayedBefore", 1);
        PlayerPrefs.Save();
    }

    public bool HasSavedName()
    {
        return !string.IsNullOrEmpty(PlayerName);
    }


    public void ResetPlayerPref()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.LogWarning("PlayerPrefs have been reset.");
    }
}
