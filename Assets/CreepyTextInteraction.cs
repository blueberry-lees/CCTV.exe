using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TMPro;
using UnityEngine;

public class CreepyTextInteraction : MonoBehaviour
{
    public GameObject creepyInteractionGroup; // Assign in inspector
    public GameObject menuGroup;
    public GameObject letsBegin;
    public GameObject check;
    public GameObject readMe;
    public GameObject continueButton;

    public TMP_Text responseText;    // Creepy message text UI (TMP)

    string fileName = "passcode.txt";
    string filePath;


    public List<FailureResponse> failureResponses;

    private int failedEditAttempts = 0;
    private string lastPlayerInput = "";

    string creepyPrompt =
@"There are rules.

Type 'EMILY' AFTER these texts to play the game.

If you don't... well,
    you wouldn't like the alternative.

This file will remember.
    Save this file when you're done. ";

    void Start()
    {
        // Save in the build folder (same as .exe)
        filePath = Path.Combine(Path.GetDirectoryName(Application.dataPath), fileName);
        UnityEngine.Debug.Log("Creepy text file path: " + filePath);

        bool fileConfirmed = false;

        if (File.Exists(filePath))
        {
            string fullContent = File.ReadAllText(filePath).ToLower();
            int okCount = 0;
            int index = 0;

            while ((index = fullContent.IndexOf("emily", index)) != -1)
            {
                okCount++;
                index += 2;
            }

            if (okCount >= 2)
            {
                fileConfirmed = true;
            }
        }

        PlayerPrefs.SetInt("fileConfirmed", fileConfirmed ? 1 : 0);
        PlayerPrefs.Save();

        //UnityEngine.Debug.Log("Has played before: " + PlayerPrefs.GetInt("hasPlayedBefore", 0));
        //UnityEngine.Debug.Log("File confirmed: " + PlayerPrefs.GetInt("fileConfirmed", 0));

        if (!fileConfirmed)
        {
            StartCoroutine(WaitTime(2f));
            creepyInteractionGroup.SetActive(true);
            menuGroup.SetActive(false);
            check.SetActive(false);
        }
        else
        {
            StartCoroutine(WaitTime(2f));
            menuGroup.SetActive(true);
            creepyInteractionGroup.SetActive(false);
            continueButton.SetActive(PlayerPrefs.GetInt("hasPlayedBefore", 0) != 0);
        }

        //if (!File.Exists(filePath))
        //{
        //    check.gameObject.SetActive(false);
        //}
    }






    public void CreateAndOpenTextFile()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, creepyPrompt);
            check.gameObject.SetActive(true);
        }

        Process.Start(new ProcessStartInfo()
        {
            FileName = "notepad.exe",
            Arguments = $"\"{filePath}\"",
            UseShellExecute = true
        });

        UnityEngine.Debug.Log("Opened creepy file.");
    }

    public void CheckPlayerResponse()
    {
        if (!File.Exists(filePath))
        {
            responseText.text = "The file is missing.\nYOU DELETED IT DIDN'T YOU. HERE. DO IT AGAIN.\n\n<i>(After editing the file, press CHECK again)</i>";
            CreateAndOpenTextFile();
            failedEditAttempts = 0; // reset if file was missing
            return;
        }

        string fullContent = File.ReadAllText(filePath);
        string prompt = creepyPrompt;
        string playerInput = "";

        if (fullContent.Length > prompt.Length)
            playerInput = fullContent.Substring(prompt.Length).ToLower();

        int okCount = 0;
        int index = 0;

        while ((index = playerInput.IndexOf("emily", index)) != -1)
        {
            okCount++;
            index += 2;
        }

        // If success
        if (okCount >= 5)
        {
            PlayerPrefs.SetInt("fileConfirmed", 1);
            PlayerPrefs.Save();

            responseText.text = $"You've typed them\n{okCount} times.\n\nEmily...\n That's better. Let's begin.";

            check.SetActive(false);
            readMe.SetActive(false);
            letsBegin.SetActive(true);
            failedEditAttempts = 0;
            lastPlayerInput = ""; // reset tracking
            return;
        }
        else if (okCount > 0) //not enough input
        {
            responseText.text = $"You only wrote it {okCount} time...\nThat's not enough.\nTry again.";
            failedEditAttempts = 0; // they made progress
            lastPlayerInput = playerInput;
            return;
        }
        else // No EMILY found at all
        {
            if (playerInput == lastPlayerInput)
                failedEditAttempts++;
            else
            {
                failedEditAttempts = 1;
                lastPlayerInput = playerInput;
            }

            FailureResponse response = failureResponses.Find(r =>
            r.attemptNumber == failedEditAttempts &&
            okCount >= r.minOkCount &&
            okCount <= r.maxOkCount
            );

            if (response != null)
            {
                responseText.text = response.message;
            }
            else
            {
                responseText.text = $"Still wrong. EMILY count: {okCount}. Keep trying.";
            }
        }
    }

    public void LetsBegin()
    {
        if (PlayerPrefs.GetInt("hasPlayedBefore", 0) != 0)
        {
            continueButton.SetActive(true);
        }
        else
        {
            continueButton.SetActive(false);
        }
        menuGroup.SetActive(true);
        creepyInteractionGroup.SetActive(false);
    }


    public IEnumerator WaitTime(float s)
    {
        yield return new WaitForSeconds(s); 
    }
}




[System.Serializable]
public class FailureResponse
{
    public int minOkCount;          // e.g., 0
    public int maxOkCount;          // e.g., 1
    public int attemptNumber;       // attempt # (e.g., 1st, 2nd, 3rd)
    [TextArea(2, 5)]
    public string message;
}