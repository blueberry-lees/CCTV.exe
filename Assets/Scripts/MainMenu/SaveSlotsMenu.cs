using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SaveSlotsMenu : MonoBehaviour
{
    [Header("Main Menu Reference")]
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private Button backButton;
    [SerializeField] private string sceneName;

    private SaveSlot[] saveSlots;
    private bool isLoadingGame;

    private void Awake()
    {
        saveSlots = GetComponentsInChildren<SaveSlot>(includeInactive: true);
    }

    public void ActivateMenu(bool isLoadingGame)
    {
        this.gameObject.SetActive(true);
        this.isLoadingGame = isLoadingGame;

        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.instance.GetAllProfilesGameData();

        int i = 0;
        foreach (SaveSlot slot in saveSlots)
        {
            string id = $"Profile{i}";
            profilesGameData.TryGetValue(id, out GameData data);

            slot.Initialize(this, id, data, isLoadingGame);
            i++;
        }
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

    public void OnBackClicked()
    {
        mainMenu.ActivateMenu();
        DeactivateMenu();
    }

    public void OnSlotSelected(SaveSlot slot)
    {
        DisableAll();

        DataPersistenceManager.instance.ChangeSelectedProfileId(slot.ProfileId);

        if (!isLoadingGame)
        {
            DataPersistenceManager.instance.NewGame();
        }

        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
    }

    private void DisableAll()
    {
        foreach (SaveSlot slot in saveSlots)
        {
            slot.SetInteractable(false);
        }
        backButton.interactable = false;
    }
}
