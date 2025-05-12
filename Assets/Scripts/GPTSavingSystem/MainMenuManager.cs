using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenuManager : MonoBehaviour
{
    public string firstSceneName = "Round_3";
    public GameObject slotSelectionPanel;
    public GameObject mainMenuPanel;

    public void StartNewGame()
    {
        // Optional: Delete all save slots
        for (int i = 0; i < 3; i++)
        {
            string path = Path.Combine(Application.persistentDataPath, $"save_slot_{i}.json");
            if (File.Exists(path))
                File.Delete(path);
        }

        SceneManager.LoadScene(firstSceneName);
    }

    public void OnSelectSaveSlot(int slot)
    {
        SaveData data = SaveSystem.LoadGame(slot);
        if (data != null)
        {
            // Load the saved scene
            SceneManager.LoadScene(data.sceneName);
        }
        else
        {
            Debug.Log("No save in this slot.");
        }
    }
    public void OpenContinueMenu()
    {
        // Activate your slot selection UI panel
        // (e.g. SlotSelectionPanel.SetActive(true);)
        slotSelectionPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void CloseContinueMenu()
    {
        slotSelectionPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
