using UnityEditor;
using UnityEngine;
using System.IO;

public class SaveFileOpener
{
    [MenuItem("Tools/Open Save File Location")]
    public static void OpenSaveFolder()
    {
        string path = Application.persistentDataPath;
        EditorUtility.RevealInFinder(path);
    }
}