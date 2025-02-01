using System.Collections.Generic;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Xml;

public class SceneManagerScript : MonoBehaviour
{
    public static SceneManagerScript instance;

    SaveData saveData;
    int activeSaveSlot = 1; //start at first slot by default

    public SaveData SaveData => saveData;

    Dictionary<string, GameObject> uniqueIDCache = new Dictionary<string, GameObject>();

    //private Dictionary<string, Vector3> scenePositions = new Dictionary<string, Vector3>();
    //private string currentSceneName;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if(saveData == null) { saveData = new SaveData(); }             //EDITOR ONLY REMOVE BEFORE BUILDING

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()                                        //MIGHT NOT NEED
    {
        saveData = SaveSystem.LoadGame(activeSaveSlot);

        if (saveData == null &&
            !(SceneManager.GetActiveScene().name == "BETA_Main Menu"))
        {
            saveData = new SaveData();

            SaveGame();
        }
    }

    // Change to a new scene and save the current scene's state
    public void ChangeScene(string newSceneName)
    {
        if(SceneManager.GetActiveScene().name == "BETA_Main Menu")
        {
           GameObject.FindWithTag("MainMenu").SetActive(false);
        }

        SaveSceneState();

        // Load the new scene
        StartCoroutine(LoadSceneRoutine(newSceneName));

        //SceneManager.LoadScene(newSceneName);
    }

    private IEnumerator LoadSceneRoutine(string newSceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(newSceneName);

        //wait until it loads to restore the scene state (i.e. move player)
        while (!asyncLoad.isDone) { yield return null; }

        yield return new WaitForSeconds(0.1f);

        RestoreSceneState();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //clear previous scene's cache
        uniqueIDCache.Clear();

        foreach (UniqueID obj in FindObjectsOfType<UniqueID>())
        {
            if (!uniqueIDCache.ContainsKey(obj.ID))
            { uniqueIDCache[obj.ID] = obj.gameObject; }
        }
    }

    // Restore the state of a scene when it is loaded
    private void RestoreSceneState()
    {
        if (saveData == null) { return; }

        //restore player position in that scene
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector3 spawnPosition;
            if (saveData.scenePositions.Exists(sp => sp.sceneName == SceneManager.GetActiveScene().name))
            {
                spawnPosition = saveData.GetPlayerPosition(SceneManager.GetActiveScene().name);
                spawnPosition = spawnPosition + Vector3.up + (player.transform.forward * 3);                //ADJUST TO AVOID SPAWNING ON TRIGGER
            }
            else
            {
                spawnPosition = player.transform.position;
            }

            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = spawnPosition;
            player.GetComponent<CharacterController>().enabled = true;

        }

        //destroy objects that were removed before
        foreach(string objectID in saveData.destroyedObjects)
        {
            GameObject obj = FindObjectByUniqueID(objectID);
            if (obj != null)
            {
                Destroy(obj);
            }
        }
    }
    // Save the state of the current scene
    private void SaveSceneState()
    {
        if (saveData == null) { return; }

        GameObject player = GameObject.FindWithTag("Player");
        if(player != null)
        {
            //saving player position to current scene
            saveData.SavePlayerPosition(SceneManager.GetActiveScene().name, player.transform.position);
        }

        SaveSystem.SaveGame(saveData, activeSaveSlot);
    }

    public void SaveGame()
    {
        saveData.currentSceneName = SceneManager.GetActiveScene().name;
        SaveSystem.SaveGame(saveData, activeSaveSlot);      //save data to specific slot
    }

    public void LoadGame(int slot)
    {
        activeSaveSlot = slot;
        saveData = SaveSystem.LoadGame(slot);
        if(saveData != null)
        {
            SceneManager.LoadScene(saveData.currentSceneName);
        }
    }

    //call when an object is destroyed to save it in memory
    public void MarkObjectAsDestroyed(string objectID)
    {
        if (saveData == null) { saveData = new SaveData(); }

        if (!saveData.destroyedObjects.Contains(objectID))
        {
            saveData.destroyedObjects.Add(objectID);
        }
    }

    private GameObject FindObjectByUniqueID(string id)
    {
        if (uniqueIDCache.ContainsKey(id))
        {
            return uniqueIDCache[id];
        }

        return null;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}










/*

using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UIElements;
using UnityEngine.ProBuilder.Shapes;

// Put this script on the object to trigger which scene to enter
public class SceneManagerScript : MonoBehaviour
{       
    public static SceneManagerScript instance;

    public string lastSpawnPoint;
    public string currentPlayerPoint;

    // write the exact name of the scene
    [Header("===== Write The Exact Name of the Scene =====")]
    [SerializeField] private string loadNextScene;

    [Header("===== GameObject that Do Not Destroy for Next Scene =====")]
    [SerializeField] private GameObject gunModelPlaceHolder;

    [Header("===== Display Timer Count Down =====")]
    [SerializeField] private TMP_Text playTimerText;
       
    private string playerTag = "Player";

    private float _playTimer = 0f;
    
    // Getter-only for gameplay time
    public float gameplayTime => _playTimer;

    // The location to save the data
    private string saveFolderPath;
    private string saveFilePath;
    public PlayerData data;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;            
        }
        
    }    
    private void Start()
    {  
        _playTimer = 0f;       
    } 

    private void Update()
    {
        // bugs if there is something here Main Menu will have Null Reference!!!!

        //_playTimer += Time.deltaTime;
        //if (playTimerText != null)
        //{
        //    playTimerText.gameObject.SetActive(true);            
        //    playTimerText.text = FormatTime(_playTimer);            
        //}

        //SaveLoadManager.instance.SaveDataWithKeyPress();
        //SaveLoadManager.instance.LoadDataWithKeyPress();        
               
    }


    public void LoadingToMenuScene()
    {
        SceneManager.LoadScene(0);
    }
    
    public void LoadBETA_MainMenu()
    {  
        SceneManager.LoadScene(1);
    }

    public void LoadBETA_ShipHub()
    {

        // if coming from outside the player need to spawn at PlayerEntrance GameObject
        // Find the CharacterController in the scene
        // CharacterController playerController = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
        // Vector3 loadPlayerPosition = LoadPlayerData(); // Get saved player position
        // playerController.transform.position = loadPlayerPosition; // Update

        GameObject.FindWithTag("MainMenu").SetActive(false);
        // BETA_ShipHub
        SceneManager.LoadScene(2);
        
    }

    public void LoadBETA_OuterShipArea()
    {       
        // BETA_Outer Ship Area
        SceneManager.LoadScene(3);

    }

    public void LoadBETA_Area1Platforms()
    {
        // BETA_Area 1-Platform
        SceneManager.LoadScene(4);
    }

    public void LoadBETA_Area2Platforms()
    {
        // BETA_Area 1-Platform
        SceneManager.LoadScene(5);      
    }


    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);  // Calculate minutes
        int seconds = Mathf.FloorToInt(time % 60F);  // Calculate seconds
        return string.Format("{0:00}:{1:00}", minutes, seconds); // Format as MM:SS
    }  
  
}
*/