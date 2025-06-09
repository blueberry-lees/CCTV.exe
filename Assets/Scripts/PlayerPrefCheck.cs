using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefCheck : MonoBehaviour
{

    public static PlayerPrefCheck instance;


    [Header("Inspect only")]

    public string playerName;
    public int hasPlayedBefore;
    public  string inkStateJSON = "";


    [Header("Last tags in ink")]
    public string lastBackground = "";
    public string lastCharacter = "";
    public string lastExpression = "";
    public string lastSpeaker = "";

    [Header("Where the next knot to go when enter story?")]
    public string returnPoint = "";

    [Header("What version should the Interface be?")]
    public int uiVersion = 1;

    [Header("Reset player prefs on start?")]
    public bool isPlayerPrefsCleared = false;



    private void Awake()
    {
        //IF AN INSTANCE ALREADY EXIST AND IT'S NOT THIS ONE, DESTORY THIS OBJ
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        //SET THE INSTANCE AND MARK AS PERSISTENT

        instance = this;
        DontDestroyOnLoad(gameObject);

    }
    void Start()
    {
        ResetPlayerPrefOnStart();
        UpdatePlayerPrefCheck();
    }

    void Update()
    {
        UpdatePlayerPrefCheck();
    }



    public void UpdatePlayerPrefCheck()
    {
        playerName =  PlayerPrefs.GetString("playerName", "");
        hasPlayedBefore = PlayerPrefs.GetInt("hasPlayedBefore", 0);
        inkStateJSON = PlayerPrefs.GetString("InkState", "");
        lastBackground = PlayerPrefs.GetString("LastBackground", "");
        lastCharacter = PlayerPrefs.GetString("LastCharacter", "");
        lastExpression = PlayerPrefs.GetString("LastExpression", "");
        lastSpeaker = PlayerPrefs.GetString("LastSpeaker", "");
        returnPoint = PlayerPrefs.GetString("ReturnPoint", "");
        uiVersion = PlayerPrefs.GetInt("UIVersion", 1);
    }


    public void ResetPlayerPrefOnStart()
    {
        if (isPlayerPrefsCleared)
        {
            Debug.Log("Clearing PREFS");
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            isPlayerPrefsCleared = false;
        }  
    }

    [ContextMenu("Reset Ink State")]
    public void ResetInkState()
    {
        PlayerPrefs.SetString("InkState", "");
    }


}
