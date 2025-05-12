using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string GetSlotPath(int slotIndex) =>
        Path.Combine(Application.persistentDataPath, $"save_slot_{slotIndex}.json");

    public static void SaveGame(int slotIndex, SaveData data)
    {
        string path = GetSlotPath(slotIndex);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        PlayerPrefs.SetInt("HasSave_" + slotIndex, 1); // Used for checking if a slot is filled
    }

    public static SaveData LoadGame(int slotIndex)
    {
        string path = GetSlotPath(slotIndex);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<SaveData>(json);
        }
        return null;
    }

    public static bool SaveExists(int slotIndex)
    {
        return File.Exists(GetSlotPath(slotIndex));
    }

    public static void DeleteSave(int slotIndex)
    {
        string path = GetSlotPath(slotIndex);
        if (File.Exists(path))
            File.Delete(path);

        PlayerPrefs.DeleteKey("HasSave_" + slotIndex);
    }
}
