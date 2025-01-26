using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using System.Collections;

// Put this script on the object to trigger which scene to enter
public class SceneManagerScript : MonoBehaviour
{    
    public static SceneManagerScript instance;

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
        instance = this;            
    }

    private void Start()
    {      
        _playTimer = 0f;        
    } 

    private void Update()
    {
        // Set the target frame rate to 30
        Application.targetFrameRate = 30;

        _playTimer += Time.deltaTime;
        if (playTimerText != null)
        {
            playTimerText.gameObject.SetActive(true);            
            playTimerText.text = FormatTime(_playTimer);            
        }

        //SaveLoadManager.instance.SaveDataWithKeyPress();
        //SaveLoadManager.instance.LoadDataWithKeyPress();
       
    }

    // Checking if object that trigger scene changes collided with player to change scene
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has the specified tag
        if (other.CompareTag(playerTag))
        {
            // Load the specified scene
            if (!string.IsNullOrEmpty(loadNextScene))
            {
                Debug.Log($"Triggering scene change to {loadNextScene}.");
                SceneManager.LoadScene(loadNextScene);
            }
            else
            {
                Debug.LogError("Scene name is not specified!");
            }
        }
    }

    // Load Scene (starting index 0)    
    public void LoadLevelOneScene()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadLevelTwoScene()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadLevelThreeScene()
    {
        SceneManager.LoadScene(3);
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);  // Calculate minutes
        int seconds = Mathf.FloorToInt(time % 60F);  // Calculate seconds
        return string.Format("{0:00}:{1:00}", minutes, seconds); // Format as MM:SS
    }  
  
}
