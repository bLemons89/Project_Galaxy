using System.Collections.Generic;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public static SceneManagerScript instance;

    //private Dictionary<string, SceneData> sceneData = new Dictionary<string, SceneData>();
    private Dictionary<string, Vector3> scenePositions = new Dictionary<string, Vector3>();
    private string currentSceneName;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
            currentSceneName = SceneManager.GetActiveScene().name;

            LoadScenePositions();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Change to a new scene and save the current scene's state
    public void ChangeScene(string newSceneName)
    {
        if(currentSceneName == "BETA_Main Menu")
        {
           GameObject.FindWithTag("MainMenu").SetActive(false);
        }

        SaveCurrentSceneState();

        // Load the new scene
        SceneManager.LoadScene(newSceneName);
        //currentSceneName = newSceneName;
    }

    // Save the state of the current scene
    private void SaveCurrentSceneState()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector3 scenePosition = 
                player.transform.position + player.transform.up + (player.transform.forward * -5f);

            scenePositions[currentSceneName] = scenePosition;
            SaveScenePositions();
        }


        /*
        if (!string.IsNullOrEmpty(currentSceneName))
        {
            SceneData data = new SceneData();

            // Example: Save player position
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                data.playerPosition = player.transform.position + (player.transform.up) + //up to prevent falling through ground
                    (player.transform.forward * -5f); //5 units back to prevent from spawning on a trigger
            }
                // Save other relevant data for the scene
                // (e.g., inventory, enemies, objects, etc.)
                sceneData[currentSceneName] = data;

        }*/
    }

    // Restore the state of a scene when it is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentSceneName = scene.name;

        GameObject player;
        while ((player = GameObject.FindWithTag("Player")) == null)
        {
            return;
        }

        LoadPlayerPosition();

        /*if (sceneData.ContainsKey(scene.name))
        {
            SceneData data = sceneData[scene.name];

            // Restore player position
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                player.transform.position = data.playerPosition;
            }

            // Restore other data for the scene
            // (e.g., enemies, objects, etc.)
        }

        // Unsubscribe from the event
        SceneManager.sceneLoaded -= OnSceneLoaded;*/
    }

    private IEnumerator RestorePlayerPosition()
    {
        yield return new WaitForSeconds(0.1f);

        GameObject player;
        while ((player = GameObject.FindWithTag("Player")) == null)
        {
            yield return null;
        }

        LoadPlayerPosition();
    }

    private void LoadPlayerPosition()
    {
        if (scenePositions.ContainsKey(currentSceneName))
        {
            Vector3 savedPosition = scenePositions[currentSceneName];

            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                player.GetComponent<CharacterController>().enabled = false;
                player.transform.position = savedPosition;
                player.GetComponent<CharacterController>().enabled = true;
            }
        }
    }

    private void SaveScenePositions()
    {
        foreach (var scene in scenePositions)
        {
            PlayerPrefs.SetFloat($"{scene.Key}_X", scene.Value.x);
            PlayerPrefs.SetFloat($"{scene.Key}_Y", scene.Value.y);
            PlayerPrefs.SetFloat($"{scene.Key}_Z", scene.Value.z);
        }
        PlayerPrefs.Save(); //ensure data is written to disk
    }

    private void LoadScenePositions()
    {
        scenePositions.Clear();
        string[] sceneNames = { "BETA_Main Menu",
                                "BETA_Outer Ship Area",
                                "BETA_Area-1-Platforms",
                                "BETA_Area-2-Industrial" }; //manually define known scene names

        foreach (var scene in sceneNames)
        {
            if (PlayerPrefs.HasKey($"{scene}_X"))
            {
                float x = PlayerPrefs.GetFloat($"{scene}_X");
                float y = PlayerPrefs.GetFloat($"{scene}_Y");
                float z = PlayerPrefs.GetFloat($"{scene}_Z");
                scenePositions[scene] = new Vector3(x, y, z);
                Debug.Log($"Loaded position for {scene}: ({x}, {y}, {z})");
            }
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

[System.Serializable]
public class SceneData
{
    public Vector3 playerPosition; // Example: Store player position

    // Add other fields as needed to save scene-specific data
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