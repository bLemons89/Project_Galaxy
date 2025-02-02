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

    [Header("===== MANAGERS =====")]
    private SceneManagerScript sceneManager;
    private ButtonFunctions buttonFunctions;
    private playerScript _playerScript;
    
    [Header("===== TEMP VARIABLES =====")]
    [SerializeField] GameObject menuActive;
    
    [Header("Cameras")]
    [SerializeField] private Camera gameCamera; 
    [SerializeField] private Camera loadingCamera;
    private Camera currentCamera;

    float timeScaleOrig;
    bool isPaused;

    // Pause Events //
    //private GameState currentGameState;
    //public delegate void GameStateChangeHandler(GameState newGameState);
    //public event GameStateChangeHandler OnGameStateChange;
    //public GameState CurrentGameState { get; private set; }

    // Getters and Setters //
    public GameObject MenuActive
    { get => menuActive; set => menuActive = value; }
    public Camera CurrentCamera
    { get => currentCamera; set => currentCamera = value; }
    public Camera GameCamera
    { get => gameCamera; set => gameCamera = value; }
    public Camera LoadingCamera
    { get => loadingCamera; set => loadingCamera = value; }
    public bool IsPaused
    { get => isPaused; set => isPaused = value; }
    public playerScript PlayerScript
    { get => _playerScript; set => _playerScript = value; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        { 
            Destroy(gameObject);
        }
        
        // Set Current GameState
        //currentGameState = GameState.Gameplay;
        //OnGameStateChange?.Invoke(currentGameState);
        timeScaleOrig = Time.timeScale;
        isPaused = false;

        // Instantiate        
        sceneManager = this.GetComponent<SceneManagerScript>();
        buttonFunctions = FindObjectOfType<ButtonFunctions>();
        _playerScript = FindObjectOfType<playerScript>();
    }
    
    // Input //
    void Update()
    {
        // Pause Input
        if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Pause"))
        {
            if (menuActive == null)
            {
                StatePause();
                menuActive = buttonFunctions.PauseMenu;
                menuActive.SetActive(true);
            }
            else if (menuActive == buttonFunctions.PauseMenu)
            {
                StateUnpause();
            }
            ///disable for main menu
            //if (currentGameState == GameState.Gameplay)
            //{
            //    HandleGameStateChange(GameState.Pause);
            //    buttonFunctions.OpenPauseMenu();                
            //}
            //else if (currentGameState == GameState.Pause)
            //{                
            //    HandleGameStateChange(GameState.Gameplay);

            //    if (menuActive != null)
            //    {
            //        buttonFunctions.CloseAllMenus();                    
            //    }                
            //}
        }
    }

    //// Game States //
    //private void HandleGameStateChange(GameState newState)
    //{
    //    // Pause //
    //    if (newState == GameState.Pause)
    //    {            
    //        currentGameState = GameState.Pause;
    //        Cursor.visible = true;
    //        Cursor.lockState = CursorLockMode.None;

    //        OnGameStateChange?.Invoke(currentGameState);            
    //    }
    //    // Unpause //
    //    else if (newState == GameState.Gameplay)
    //    {            
    //        currentGameState = GameState.Gameplay;
    //        Cursor.visible = false;
    //        Cursor.lockState = CursorLockMode.Locked;

    //        OnGameStateChange?.Invoke(currentGameState);
    //    }
    //}

    public void StatePause()
    {
        //toggle
        isPaused = !isPaused;
        Time.timeScale = 0;
        //cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined; //none can go outside the window/app
    }
    public void StateUnpause()
    {
        //toggle
        isPaused = !isPaused;
        Time.timeScale = timeScaleOrig;
        //cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //unpause, reset temp variable
        menuActive.SetActive(false);
        menuActive = null;
    }

    // Pause Buttons //
    public void Resume()
    {
        StateUnpause();
        //HandleGameStateChange(GameState.Gameplay);
        buttonFunctions.ClosePauseMenu();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        _playerScript.Respawn();
        StateUnpause();        
    }
    public void SaveGame()
    {
        StartCoroutine(buttonFunctions.SaveMenuButton());

        //prompt for overwrite, or confirm 
        // call save method
        SceneManagerScript.instance.SaveGame();

        // Stamp
        //string timeStamp = System.DateTime.Now.ToString();
        //buttonFunctions.TimeDateStamp.text = timeStamp;
    }

    public void LoadGame()
    {
        SceneManagerScript.instance.LoadGame(1);            //takes an int for the slot number (i.e. 1, 2, or 3)
    }

    public void MainMenu()
    {
        //Navigate to Main Menu Scene
        //remember to disable input?
        SceneManager.LoadScene(1);
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
