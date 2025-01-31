using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //singleton
    public static GameManager instance;
    private GameState currentGameState;

    [Header("===== MANAGERS =====")]
    private AudioManager audioManager;
    private SceneManagerScript sceneManager;
    private ButtonFunctions buttonFunctions;
    private playerScript _playerScript;
    
    [Header("===== TEMP VARIABLES =====")]
    [SerializeField] GameObject menuActive;
    
    [Header("Cameras")]
    [SerializeField] private Camera gameCamera; 
    [SerializeField] private Camera loadingCamera;
    private Camera currentCamera;

    // Pause Events //
    public delegate void GameStateChangeHandler(GameState newGameState);
    public event GameStateChangeHandler OnGameStateChange;

    // Getters and Setters //
    public GameState CurrentGameState { get; private set; }

    public GameObject MenuActive
    { get => menuActive; set => menuActive = value; }
    public Camera CurrentCamera
    { get => currentCamera; set => currentCamera = value; }
    public Camera GameCamera
    { get => gameCamera; set => gameCamera = value; }
    public Camera LoadingCamera
    { get => loadingCamera; set => loadingCamera = value; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // Commented this out The scene transition should work normally, just put all the required SerializeField on each level. 
            DontDestroyOnLoad(gameObject);
        }
        else 
        { 
            Destroy(gameObject);
        }
        // Set Current GameState
        currentGameState = GameState.Gameplay;
        OnGameStateChange?.Invoke(currentGameState);

        // Instantiate        
        sceneManager = this.GetComponent<SceneManagerScript>();
        audioManager = this.GetComponent<AudioManager>();
        buttonFunctions = FindObjectOfType<ButtonFunctions>();
        _playerScript = FindObjectOfType<playerScript>();
        
    }
    
    // Input //
    void Update()
    {
        // Pause Input
        if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Pause"))
        {
            ///disable for main menu
            if (currentGameState == GameState.Gameplay)
            {
                HandleGameStateChange(GameState.Pause);
                buttonFunctions.OpenPauseMenu();                
            }
            else if (currentGameState == GameState.Pause)
            {                
                HandleGameStateChange(GameState.Gameplay);

                if (menuActive != null)
                {
                    buttonFunctions.CloseAllMenus();                    
                }                
            }
        }
    }

    // Game States //
    private void HandleGameStateChange(GameState newState)
    {
        // Pause //
        if (newState == GameState.Pause)
        {            
            currentGameState = GameState.Pause;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
            OnGameStateChange?.Invoke(currentGameState);            
        }
        // Unpause //
        else if (newState == GameState.Gameplay)
        {            
            currentGameState = GameState.Gameplay;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            OnGameStateChange?.Invoke(currentGameState);
        }
    }
    // Pause Buttons //
    public void Resume()
    {
        //GameManager.instance.StateUnPause();
        HandleGameStateChange(GameState.Gameplay);
        buttonFunctions.ClosePauseMenu();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        _playerScript.Respawn();
        //GameManager.instance.StateUnPause();        
    }
    public void SaveGame()
    {
        StartCoroutine(buttonFunctions.SaveMenuButton());

        //prompt for overwrite, or confirm 
        // call save method
        // Stamp
        //string timeStamp = System.DateTime.Now.ToString();
        //buttonFunctions.TimeDateStamp.text = timeStamp;
    }

    public void MainMenu()
    {
        //Navigate to Main Menu Scene
        //remember to disable input?
    }

    public void Quit()
    {
        #if UNITY_EDITOR
                // Stop play mode in the editor
                UnityEditor.EditorApplication.isPlaying = false;
        #endif

        #if UNITY_WEBGL
            // reload the page on quit
            Application.OpenURL(Application.absoluteURL); // This reloads the page, effectively restarting the game
        #endif

        #if !UNITY_EDITOR && !UNITY_WEBGL
            Application.Quit();
        #endif
    }

}
