using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.IO;

public class OpenTextInNotepad : MonoBehaviour
{
    string fileName = "readme.txt";
    string filePath;
    string creepyPrompt =
@"There are rules.

Type OK below to agree.

If you don't... well,
you wouldn't like the alternative.

This file will remember.";

    void Start()
    {
        filePath = Path.Combine(Application.dataPath, fileName);
    }

    public void CreateText()
    {

        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath,creepyPrompt);
        }

        // Launch Notepad
        Process.Start(new ProcessStartInfo()
        {
            FileName = "notepad.exe",
            Arguments = $"\"{filePath}\"",
            UseShellExecute = true
        });

        UnityEngine.Debug.Log("File opened. Waiting for player to respond...");
    }

    public void CheckIfPlayerResponded()
    {
        if (File.Exists(filePath))
        {
            string content = File.ReadAllText(filePath);

            if (content.ToLower().Contains("ok"))
            {
                UnityEngine.Debug.Log("Player confirmed by typing OK!");
                PlayerPrefs.SetInt("fileConfirmed", 1);
            }
            else
            {
                UnityEngine.Debug.Log("Player has not typed OK yet.");
            }
        }
        else
        {
            UnityEngine.Debug.LogWarning("File not found.");
        }
    }
}
