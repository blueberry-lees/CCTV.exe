using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayPlayerName : MonoBehaviour
{
    string playerName;
    TMP_Text playerNameDisplay;


    private void Awake()
    {
        playerNameDisplay = GetComponent<TMP_Text>();
         playerName = PlayerPrefs.GetString("playerName", "");
        playerNameDisplay.text = playerName;
    }
}
