using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSlotManager : MonoBehaviour
{
    public GameObject saveSlotPrefab;
    public Transform slotParent;

    void Start()
    {
        GenerateSaveSlots();
    }

    void GenerateSaveSlots()
    {
        for (int i = 0; i < 3; i++) // 3 slots
        {
            GameObject slotObj = Instantiate(saveSlotPrefab, slotParent);
            SaveSlotUI ui = slotObj.GetComponent<SaveSlotUI>();

            SaveData data = SaveSystem.LoadGame(i);
            ui.Setup(i, data, OnSelectSlot);
        }
    }

    void OnSelectSlot(int slotIndex)
    {
        SaveData data = SaveSystem.LoadGame(slotIndex);
        if (data != null)
        {
            SceneManager.LoadScene(data.sceneName);
        }
        else
        {
            Debug.Log("No save found.");
        }
    }
}
