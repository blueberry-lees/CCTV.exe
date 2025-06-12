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


        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("playerName", "")))
        {
            playerName = PlayerPrefs.GetString("playerName", "");
        }
        else playerName = "\"Nobody\"";

            playerNameDisplay.text = playerName;
    }
}
