using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FileResponseChecker : MonoBehaviour
{
    public GameObject proceedButton; // the thing to enable
    public TMP_Text warningText;     // optional creepy message

    public void CheckResponse()
    {
        if (PlayerPrefs.GetInt("fileConfirmed", 0) == 1)
        {
            proceedButton.SetActive(true);
            warningText.text = ""; // or hide
        }
        else
        {
            warningText.text = "You didn't agree yet.\nThe file is waiting.";
        }
    }
}
