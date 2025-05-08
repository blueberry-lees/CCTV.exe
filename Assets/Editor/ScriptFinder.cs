using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ScriptFinder : EditorWindow
{
    [MenuItem("Tools/List All Scripts In Scene")]
    public static void ListAllScriptsInScene()
    {
        MonoBehaviour[] allScripts = GameObject.FindObjectsOfType<MonoBehaviour>();
        HashSet<string> scriptNames = new HashSet<string>();

        foreach (MonoBehaviour script in allScripts)
        {
            if (script != null)
                scriptNames.Add(script.GetType().Name);
        }

        Debug.Log("Scripts used in the current scene:");
        foreach (string name in scriptNames)
        {
            Debug.Log(name);
        }
    }
}
