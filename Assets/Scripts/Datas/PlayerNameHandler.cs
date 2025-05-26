using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerNameHandler
{
    private const string playerNameKey = "playerName";
    private const string hasPlayedKey = "hasPlayedBefore";

    public static string playerName
    {
        get => PlayerPrefs.GetString(playerNameKey, "");
        private set => PlayerPrefs.SetString(playerNameKey, value);
    }

    public static void SavePlayerName(string name)
    {
        playerName = name;
        PlayerPrefs.SetInt(hasPlayedKey, 1);
        PlayerPrefs.Save();
    }

    public static bool HasSavedName()
    {
        return !string.IsNullOrEmpty(playerName);
    }

    public static void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        UnityEngine.Debug.LogWarning("PlayerPrefs have been reset.");
    }
}
