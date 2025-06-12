using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string nextScene;
    public GameObject playerInstruction;
   public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        GameState.ClearDialogueHistory();
        SceneManager.LoadScene(nextScene);
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(nextScene);
    }

    public void HowToPlay()
    {
        playerInstruction.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
