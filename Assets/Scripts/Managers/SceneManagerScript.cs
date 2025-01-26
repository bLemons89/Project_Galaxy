using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;


// Put this script on the object to trigger which scene to enter
public class SceneManagerScript : MonoBehaviour
{    
    public static SceneManagerScript instance;

    [SerializeField] private string sceneToLoad;

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
        instance = this;            
    }

    private void Start()
    {
        _playTimer = 0f;        
    } 

    private void Update()
    {
        _playTimer += Time.deltaTime;
        if (playTimerText != null)
        {
            playTimerText.gameObject.SetActive(true);            
            playTimerText.text = FormatTime(_playTimer);            
        }

        SaveLoadManager.instance.SaveDataWithKeyPress();
        SaveLoadManager.instance.LoadDataWithKeyPress();
    }

    // Checking if object collide with player to change scene
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

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);  // Calculate minutes
        int seconds = Mathf.FloorToInt(time % 60F);  // Calculate seconds
        return string.Format("{0:00}:{1:00}", minutes, seconds); // Format as MM:SS
    }
  
}
