using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Navigation")]
    [SerializeField] private SaveSlotsMenu saveSlotsMenu;

    [SerializeField] private GameObject continueButton;

    private void OnEnable()
    {
        if (DataPersistenceManager.instance != null)
        {
            DataPersistenceManager.instance.OnDataLoaded += OnDataLoaded;
        }
    }

    private void OnDisable()
    {
        if (DataPersistenceManager.instance != null)
        {
            DataPersistenceManager.instance.OnDataLoaded -= OnDataLoaded;
        }
    }

    private void OnDataLoaded()
    {
        // Only run this AFTER data is fully loaded
        continueButton.SetActive(DataPersistenceManager.instance.HasGameData());
        Debug.Log("MainMenu received data loaded event.");
    }

    public void OnNewGameClicked()
    {
        //create a new game -  which will initialize our game data
        DataPersistenceManager.instance.NewGame();
        //Load the gameplay scene -  which will in turn save the game because of
        //OnSceneUnloaded() in the DataPersistenceManager
        SceneManager.LoadSceneAsync("Round_3");
    }

    public void OnContinueGameClicked()
    {
        //load the next scene -  which will in turn load the game because of 
        //OnSceneLoaded() in the DataPersistenceManager
        SceneManager.LoadSceneAsync("Round_3");

    }
}
