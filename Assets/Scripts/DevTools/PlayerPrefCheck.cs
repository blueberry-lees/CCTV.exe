using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefCheck : MonoBehaviour
{
    public static PlayerPrefCheck instance;

    [Header("Inspect only (Auto-Fetched from PlayerPrefs)")]
    public string playerName;
    public int hasPlayedBefore;
  


    [Header("DEV TOOLS - Overwrite on Play")]
    public bool overwritePrefsOnStart = false;
    public string dev_playerName = "DevPlayer";
    public int dev_hasPlayedBefore = 1;

    [Header("Reset ALL PlayerPrefs on Start")]
    public bool isPlayerPrefsCleared = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        ResetPlayerPrefOnStart();
        if (overwritePrefsOnStart) OverwritePrefsFromDev();
        UpdatePlayerPrefCheck();
    }

    void Update()
    {
        UpdatePlayerPrefCheck();
        //UnityEngine.Debug.Log("Has played before: " + PlayerPrefs.GetInt("hasPlayedBefore", 0));
        //UnityEngine.Debug.Log("File confirmed: " + PlayerPrefs.GetInt("fileConfirmed", 0));
    }

    public void UpdatePlayerPrefCheck()
    {
        playerName = PlayerPrefs.GetString("playerName", "");
        hasPlayedBefore = PlayerPrefs.GetInt("hasPlayedBefore", 0);
        
   
    }

    public void ResetPlayerPrefOnStart()
    {
        if (isPlayerPrefsCleared)
        {
            Debug.LogWarning("Clearing ALL PlayerPrefs...");
            PlayerPrefs.DeleteAll();
            DialogueHistoryStatic.ClearDialogueHistory(); // optional, if you have this
            PlayerPrefs.Save();
            isPlayerPrefsCleared = false;
        }
    }

    public void OverwritePrefsFromDev()
    {
        Debug.Log("Overwriting PlayerPrefs from Dev settings...");

        PlayerPrefs.SetString("playerName", dev_playerName);
        PlayerPrefs.SetInt("hasPlayedBefore", dev_hasPlayedBefore);
      
        PlayerPrefs.Save();
    }

    [ContextMenu("Reset Ink State")]
    public void ResetInkState()
    {
        PlayerPrefs.SetString("InkState", "");
        Debug.Log("Ink state cleared.");
    }

    [ContextMenu("Force Overwrite PlayerPrefs")]
    public void DevForceOverwrite()
    {
        OverwritePrefsFromDev();
        UpdatePlayerPrefCheck();
        Debug.Log("Prefs manually overwritten from dev fields.");
    }

 
   
}
