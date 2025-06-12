using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TMPro;
using UnityEngine;

public class CreepyTextInteraction : MonoBehaviour
{
    public GameObject proceedButton; // Assign in inspector
    public TMP_Text responseText;    // Creepy message text UI (TMP)

    string fileName = "readme.txt";
    string filePath;

    string creepyPrompt =
@"There are rules.

Type OK anywhere and as much as you want to play the game.

If you don't... well,
you wouldn't like the alternative.

This file will remember.
Remember to save this file when you're done. ";

    void Start()
    {
        // Use persistentDataPath instead of dataPath
        filePath = Path.Combine(Application.persistentDataPath, fileName);

        // Log path so you can find the file
        UnityEngine.Debug.Log("Creepy text file path: " + filePath);
    }

    public void CreateAndOpenTextFile()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, creepyPrompt);
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
            responseText.text = "The file is missing.\nThat’s worse.";
            return;
        }

        string fullContent = File.ReadAllText(filePath);
        string prompt = creepyPrompt;
        string playerInput = "";

        if (fullContent.Length > prompt.Length)
            playerInput = fullContent.Substring(prompt.Length).ToLower();

        int okCount = 0;
        int index = 0;

        while ((index = playerInput.IndexOf("ok", index)) != -1)
        {
            okCount++;
            index += 2;
        }

        if (okCount >= 2)
        {
            PlayerPrefs.SetInt("fileConfirmed", 1);
            proceedButton.SetActive(true);

            responseText.text =
                $"I saw it.\n{okCount} times.\n\n" +
                "That’s... enthusiastic.\n" +
                "Fine. We begin.";
        }
        else
        {
            responseText.text =
                $"You only wrote it {okCount} time(s)...\n" +
                "One is not enough.\nTry again.";
        }
    }


}
