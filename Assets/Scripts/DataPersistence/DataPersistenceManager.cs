using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

public class DataPersistenceManager : MonoBehaviour
{
    public event Action OnDataLoaded; //chat gpt fix on continue button not showing even when there's game data 


    [Header("Debugging")]
    [SerializeField] private bool initializeDataIfNull = false;



    [Header("File Storage Config")]

    [SerializeField] private string fileName;


    private GameData gameData;

    private List<IDataPersistence> dataPersistenceObjects;

    private FileDataHandler dataHandler;

    private string selectedProfileId = "test";
    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one Data Persistence Manager in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;

        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        SaveGame();
    }


    public void ChangeSelectedProfileId(string newProfiled)
    {

        //update the profile to use for saving and loading
        this.selectedProfileId = newProfiled;
        //load the game, which will use that profile, updating our game data accordingly
        LoadGame();
    }



    public void NewGame()
    {
        this.gameData = new GameData();
        PlayTimeTracker.Instance?.StartTracking(); // start fresh session time

    }


    public void LoadGame()
    {
        //Load any saved data from a file using the data handler
        this.gameData = dataHandler.Load(selectedProfileId);

        //start a new game if the data is null and we're configured to initialize data for debugging purposes
        if (this.gameData == null && initializeDataIfNull)
        {
            NewGame();
        }

        //if no data can be loaded, dont continue
        if (this.gameData == null)
        {
            Debug.Log("No data was found. A new Game needs to be started before data can be loaded.");
            return;
        }

        //push the loaded data to all other scripts that need it
        foreach (IDataPersistence dataPersistencObj in dataPersistenceObjects)
        {
            if (dataPersistencObj != null)
            {
                dataPersistencObj.SaveData(ref gameData);
            }
            else
            {
                Debug.LogWarning("Found null IDataPersistence object in list.");
            }
        }

        PlayTimeTracker.Instance?.StartTracking(); // resume timing

        // Notify listeners that loading is done
        OnDataLoaded?.Invoke();
    }




    public void SaveGame()
    {
        // Add current session time before saving
        if (PlayTimeTracker.Instance != null && this.gameData != null)
        {
            float sessionTime = PlayTimeTracker.Instance.StopTracking();
            this.gameData.totalPlayTimeSeconds += sessionTime;
        }


        //chatgpt fix to 'null reference' here
        if (dataPersistenceObjects == null || dataPersistenceObjects.Count == 0)
        {
            dataPersistenceObjects = FindAllDataPersistenceObjects();
        }
        //if we dont have any data to save, log a warnbing here
        if (this.gameData ==null)
        {
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
            return;
        }

        Debug.Log($"dataPersistenceObjects is {(dataPersistenceObjects == null ? "null" : "not null")} and has {dataPersistenceObjects?.Count} objects.");

        //pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistencObj in dataPersistenceObjects)
        {
            dataPersistencObj.SaveData(ref gameData);
        }

        //save that data to a file using the data handler
        dataHandler.Save(gameData, selectedProfileId);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<IDataPersistence>()
            .Where(obj => obj != null);

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public bool HasGameData()
    {
        return gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }

}
