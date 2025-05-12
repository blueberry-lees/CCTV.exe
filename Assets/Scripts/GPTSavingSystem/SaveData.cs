using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string sceneName;
    public string playerName;
    public string timestamp;
    public string customDescription; // Optional: e.g., "Chapter 2 - Elevator"
                                     // Add other variables as needed (Ink variables, flags, etc.)

    public SaveData(string scene, Vector3 pos, string name)
    {
        sceneName = scene;
        playerName = name;
        timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
