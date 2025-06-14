using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string nextScene;
    //public GameObject continueButton;
    public GameObject playerInstruction;

    //void Start()
    //{
    //    // Show continue button only if player has played before
    //    if (PlayerPrefs.GetInt("hasPlayedBefore", 0) != 0)
    //    {
    //        continueButton.SetActive(true);
    //    }
    //    else
    //    {
    //        continueButton.SetActive(false);
    //    }
    //}
    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        DialogueHistoryStatic.ClearDialogueHistory();
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
