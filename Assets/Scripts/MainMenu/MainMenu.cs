using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Navigation")]
    [SerializeField] private SaveSlotsMenu saveSlotsMenu;

    [SerializeField] private GameObject continueButton;

    [SerializeField] private GameObject loadGameButton;

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
        loadGameButton.SetActive(DataPersistenceManager.instance.HasGameData());
        Debug.Log("MainMenu received data loaded event.");
    }

    public void OnNewGameClicked()
    {
        saveSlotsMenu.ActivateMenu(false);
        this.DeactivateMenu();
    }


    public void OnLoadGameClicked()
    {
        saveSlotsMenu.ActivateMenu(true);
        this.DeactivateMenu();
    }

    public void OnContinueGameClicked()
    {
        //load the next scene -  which will in turn load the game because of 
        //OnSceneLoaded() in the DataPersistenceManager
        SceneManager.LoadSceneAsync("Round_3");

    }

    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);
    }

    public void DeactivateMenu()
    {
        this .gameObject.SetActive(false);
    }

}
