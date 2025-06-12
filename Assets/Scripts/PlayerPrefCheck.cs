using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefCheck : MonoBehaviour
{
    public static PlayerPrefCheck instance;

    [Header("Inspect only (Auto-Fetched from PlayerPrefs)")]
    public string playerName;
    public int hasPlayedBefore;
    public string inkStateJSON = "";

    public string lastBackground = "";
    public string lastCharacter = "";
    public string lastExpression = "";
    public string lastSpeaker = "";

    public string returnPoint = "";
    public int uiVersion = 1;
    public int fileConfirmed = 0;

    public int trust = 5;
    public int delusion = 5; ////////////////////////////

    [Header("DEV TOOLS - Overwrite on Play")]
    public bool overwritePrefsOnStart = false;
    public string dev_playerName = "DevPlayer";
    public int dev_hasPlayedBefore = 1;
    [TextArea(2, 5)] public string dev_inkStateJSON = "";

    public string dev_lastBackground = "";
    public string dev_lastCharacter = "";
    public string dev_lastExpression = "";
    public string dev_lastSpeaker = "";

    public string dev_returnPoint = "";
    public int dev_uiVersion = 1;

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
    }

    public void UpdatePlayerPrefCheck()
    {
        playerName = PlayerPrefs.GetString("playerName", "");
        hasPlayedBefore = PlayerPrefs.GetInt("hasPlayedBefore", 0);
        inkStateJSON = PlayerPrefs.GetString("InkState", "");
        lastBackground = PlayerPrefs.GetString("LastBackground", "");
        lastCharacter = PlayerPrefs.GetString("LastCharacter", "");
        lastExpression = PlayerPrefs.GetString("LastExpression", "");
        lastSpeaker = PlayerPrefs.GetString("LastSpeaker", "");
        returnPoint = PlayerPrefs.GetString("ReturnPoint", "");
        uiVersion = PlayerPrefs.GetInt("UIVersion", 1);


        fileConfirmed = PlayerPrefs.GetInt("fileConfirmed", 0);

        trust = PlayerPrefs.GetInt("Trust", 5);      // fallback to INITIAL_SWING
        delusion = PlayerPrefs.GetInt("Delusion", 5);

   
    }

    public void ResetPlayerPrefOnStart()
    {
        if (isPlayerPrefsCleared)
        {
            Debug.LogWarning("Clearing ALL PlayerPrefs...");
            PlayerPrefs.DeleteAll();
            GameState.ClearDialogueHistory(); // optional, if you have this
            PlayerPrefs.Save();
            isPlayerPrefsCleared = false;
        }
    }

    public void OverwritePrefsFromDev()
    {
        Debug.Log("Overwriting PlayerPrefs from Dev settings...");

        PlayerPrefs.SetString("playerName", dev_playerName);
        PlayerPrefs.SetInt("hasPlayedBefore", dev_hasPlayedBefore);
        PlayerPrefs.SetString("InkState", dev_inkStateJSON);
        PlayerPrefs.SetString("LastBackground", dev_lastBackground);
        PlayerPrefs.SetString("LastCharacter", dev_lastCharacter);
        PlayerPrefs.SetString("LastExpression", dev_lastExpression);
        PlayerPrefs.SetString("LastSpeaker", dev_lastSpeaker);
        PlayerPrefs.SetString("ReturnPoint", dev_returnPoint);
        PlayerPrefs.SetInt("UIVersion", dev_uiVersion);
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

    [ContextMenu("Print Current PlayerPrefs")]
    public void PrintPrefs()
    {
        Debug.Log($"[PlayerPrefs] Name: {playerName}, PlayedBefore: {hasPlayedBefore}, ReturnPoint: {returnPoint}, UI: {uiVersion}");
    }

}
