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
    private string saveFolderPath = "SaveGame";
    private string saveFilePath;

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

        if (Input.GetKeyDown(KeyCode.O))
        {
            CreateSaveGameFolder(); 
            saveFilePath = Path.Combine("SaveGame/Save.json");
            GameObject player = GameObject.FindWithTag("Player");
            SavePlayerData(player.transform.position, playTimer);
            //string saveTimeStamp = System.DateTime.Now.ToString();            
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadGame();
        }
    }

    private void CreateSaveGameFolder()
    {
        if (!Directory.Exists(saveFolderPath))
        {
            Directory.CreateDirectory(saveFolderPath);
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
    public void SavePlayerData(Vector3 position, float playTimer)
    {
        // Find the CharacterController in the scene
        CharacterController playerController = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
        HealthSystem healthEvent = playerController.GetComponent<HealthSystem>();

        try
        {
            PlayerData data = new PlayerData(position);
            data.playTimer = playTimer;
            data.CurrentHealth = healthEvent.CurrentHealth; // Current health is not getting the correct data.
            string json = JsonUtility.ToJson(data, true); // Convert to JSON string with pretty print

            File.WriteAllText(saveFilePath, json); // Write JSON string to file
            Debug.Log($"Player location saved to {saveFilePath}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error during saving: {ex.Message}");
        }
    }

    // Load player position from a JSON file
    public Vector3 LoadPlayerData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath); // Read JSON string from file
            PlayerData data = JsonUtility.FromJson<PlayerData>(json); // Convert JSON string back to PlayerData

            Vector3 position = new Vector3(data.x, data.y, data.z);
            Debug.Log("Player location loaded: " + position + "Timer: " + FormatTime(data.playTimer));
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
        GameObject player = GameObject.FindWithTag("Player");
        SavePlayerData(player.transform.position, playTimer);
    }

    public void LoadGame()
    {
        Vector3 loadPlayerPosition = LoadPlayerData(); // Get saved player position
        Debug.Log($"Loaded Player Position: {loadPlayerPosition}");

        // Find the CharacterController in the scene
        CharacterController playerController = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
        
        if (playerController != null)
        {
            // Disable the CharacterController temporarily to update its position
            playerController.enabled = false;
            playerController.transform.position = loadPlayerPosition; // Update position
            playerController.enabled = true;

            Debug.Log($"Player position updated to: {loadPlayerPosition}");
        }
        else
        {
            Debug.LogError("CharacterController not found in the scene!");
        }
    }
}
