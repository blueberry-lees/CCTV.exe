using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class DialogueHistoryFileHandler
{
    private static string fileName = "Stenograph.txt";

    // Get the full file path for saving/loading
    private static string GetFilePath()
    {
        // Application.persistentDataPath is a safe place to store files that persist between sessions
        return Path.Combine(Application.persistentDataPath, fileName);
    }

    // Save the text content to file
    public static void Save(string content)
    {
        string path = GetFilePath();

        try
        {
            File.WriteAllText(path, content);
            //Debug.Log($"Dialogue history saved to {path}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save dialogue history: {e.Message}");
        }
    }

    // Load text content from file, or return empty if file doesn't exist
    public static string Load()
    {
        string path = GetFilePath();

        if (File.Exists(path))
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load dialogue history: {e.Message}");
                return "";
            }
        }
        else
        {
            Debug.Log("Dialogue history file not found, returning empty.");
            return "";
        }
    }

    // Optional: Delete the file (clear history permanently)
    public static void Delete()
    {
        string path = GetFilePath();

        if (File.Exists(path))
        {
            try
            {
                File.Delete(path);
                Debug.Log("Dialogue history file deleted.");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to delete dialogue history file: {e.Message}");
            }
        }
    }
}
