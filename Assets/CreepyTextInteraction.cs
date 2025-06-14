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

        UnityEngine.Debug.Log("Has played before: " + PlayerPrefs.GetInt("hasPlayedBefore", 0));
        UnityEngine.Debug.Log("File confirmed: " + PlayerPrefs.GetInt("fileConfirmed", 0));

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

        if (!File.Exists(filePath))
        {
            check.gameObject.SetActive(false);
        }
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
            responseText.text = "The file is missing.\nYOU DELETED IT DIDN'T YOU. HERE. DO IT AGAIN.";
            CreateAndOpenTextFile();
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

        if (okCount >= 2)
        {
            PlayerPrefs.SetInt("fileConfirmed", 1);
            PlayerPrefs.Save();


            responseText.text =
                $"I saw it. You've typed them\n{okCount} times.\n\n" +
                "Emily...\n" +
                "Fine. We begin.";
            

            check.SetActive(false);
            readMe.SetActive(false);
            letsBegin.SetActive(true);
        }
        else
        {
            responseText.text =
                $"You only wrote it {okCount} time...\n" +
                "That's not enough.\nTry again.";
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
