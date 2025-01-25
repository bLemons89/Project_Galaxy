using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using System.Collections;
using TMPro;
using UnityEditor;
using System.IO;
using System.Xml;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Unity.VisualScripting;


// Put this script on the object to trigger which scene to enter
public class SceneManagerScript : MonoBehaviour
{
    public static SceneManagerScript instance;

    [SerializeField] private string sceneToLoad;

    [Header("===== Display Timer Count Down =====")]
    [SerializeField] private TMP_Text playTimerText;

    private string playerTag = "Player";

    private float playTimer;

    // The location to save the data
    private string saveFolderPath;
    private string saveFilePath;
    public PlayerData data;

    private void Awake()
    {
        instance = this;            
    }

    private void Start()
    {        
        playTimer = 0f;        
    } 

    private void Update()
    {
        playTimer += Time.deltaTime;
        if (playTimerText != null)
        {
            playTimerText.gameObject.SetActive(true);            
            playTimerText.text = FormatTime(playTimer);            
        }

        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    // Define the folder path in the curent directory
        //    saveFolderPath = Path.Combine(System.Environment.CurrentDirectory, "SaveGame");

        //    // Create the SaveGame folder if it doesn't exist
        //    if (!Directory.Exists(saveFolderPath))
        //    {
        //        Directory.CreateDirectory(saveFolderPath);
        //    }
        //    saveFilePath = Path.Combine(saveFolderPath, "Save.json");

        //    GameObject player = GameObject.FindWithTag("Player");
        //    SavePlayerData(player.transform.position, playTimer);
        //    //string saveTimeStamp = System.DateTime.Now.ToString();            
        //}

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadGame();
        }
    }

  
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has the specified tag
        if (other.CompareTag(playerTag))
        {
            // Load the specified scene
            if (!string.IsNullOrEmpty(sceneToLoad))
            {
                Debug.Log($"Triggering scene change to {sceneToLoad}.");
                SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                Debug.LogError("Scene name is not specified!");
            }
        }
    }

    // Load Menu Scene (or scene index 0)    
    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene(0);
    }

    private void RestingData()
    {
        InventoryManager.instance.InventorySlotsList.Clear();
    }

    // Save player position to a JSON file
    private void SavePlayerData(Vector3 position, float playTimer)
    { 
        // Find the CharacterController in the scene
        //CharacterController playerController = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
        //HealthSystem healthEvent = playerController.GetComponent<HealthSystem>();

        data = new PlayerData(position);
        data.playTimer = playTimer;
        //data.CurrentHealth = healthEvent.CurrentHealth; // Current health is not getting the correct data.
        string json = JsonUtility.ToJson(data, true); // Convert to JSON string with pretty print

        File.WriteAllText(saveFilePath, json); // Write JSON string to file    
        
    }

    // Load player position from a JSON file
    private Vector3 LoadPlayerData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath); // Read JSON string from file
            data = JsonUtility.FromJson<PlayerData>(json); // Convert JSON string back to PlayerData

            Vector3 position = new Vector3(data.x, data.y, data.z);
            //Debug.Log("Player location loaded: " + position + "Timer: " + FormatTime(data.playTimer));
            return position;
        }
        else
        {
            Debug.LogWarning("Save file not found. Returning default position.");
            return Vector3.zero; // Default position TODO: Put the correct starting point
        }
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);  // Calculate minutes
        int seconds = Mathf.FloorToInt(time % 60F);  // Calculate seconds
        return string.Format("{0:00}:{1:00}", minutes, seconds); // Format as MM:SS
    }

    private void SavePlayerHealth()
    {
        

    }

    public void SaveGame()
    {
        // Define the folder path in the curent directory
        saveFolderPath = Path.Combine(System.Environment.CurrentDirectory, "SaveGame");

        // Create the SaveGame folder if it doesn't exist
        if (!Directory.Exists(saveFolderPath))
        {
            Directory.CreateDirectory(saveFolderPath);
        }
        saveFilePath = Path.Combine(saveFolderPath, "Save.json");
        Debug.Log($"Player location saved to {saveFolderPath}/+{saveFilePath} ");
        Debug.Log("Saving");

        GameObject player = GameObject.FindWithTag("Player");
        SavePlayerData(player.transform.position, playTimer);
    }

    public void LoadGame()
    {       
        Vector3 loadPlayerPosition = LoadPlayerData(); // Get saved player position
        Debug.Log($"Loading Data");

        // Find the CharacterController in the scene
        CharacterController playerController = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
        
        if (playerController != null)
        {
            // Disable the CharacterController temporarily to update its position
            playerController.enabled = false;
            playerController.transform.position = loadPlayerPosition; // Update position from json file data
            playerController.enabled = true;

            Debug.Log($"Player position updated to: {loadPlayerPosition}");
        }
        else
        {
            Debug.LogError("CharacterController not found in the scene!");
        }
    }
}
