using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    //singleton
    public static GameManager instance;
    ButtonFunctions buttonFunctions;
    //RadialMenu radialMenu;

    [Header("===== MANAGERS =====")]
    private GameObject gameManager;
    private SceneManagerScript sceneManager;
    private AudioManager audioManager;

    [Header("===== PLAYER =====")]
    private GameObject player;
    private playerScript playerScript;
    [SerializeField] GameObject playerDamageScreen;
    //[SerializeField] GameObject radialMenuObject;

    [Header("===== TEMP VARIABLES =====")]
    [SerializeField] GameObject menuActive;
    //[SerializeField] GameObject menuWin;
    //SerializeField] GameObject menuLose;

    [Header("===== MUSIC =====")]
    //[SerializeField] private AudioClip playMusic;

    // Flags //
    private bool isPaused;
    //private bool radialIsOpen

    // Cache //
    float timeScaleOrig;

    // Getters and Setters //
    public bool IsPaused
    {get => isPaused; set => isPaused = value;}
    public GameObject MenuActive
    { get => menuActive; set => menuActive = value; }

    public GameObject PlayerDamageScreen
    {get => playerDamageScreen; set => playerDamageScreen = value;}   
    public playerScript PlayerScript
    { get => playerScript; set => playerScript = value; }
    public GameObject Player => player;

    void Awake()
    {
        instance = this;
        buttonFunctions = FindObjectOfType<ButtonFunctions>();
        //radialMenu = FindObjectOfType<RadialMenu>();

        // set original values
        timeScaleOrig = Time.timeScale;
        //radialIsOpen = false;

        gameManager = GameObject.FindWithTag("GameManager");
        sceneManager = gameManager.GetComponent<SceneManagerScript>();
        audioManager = gameManager.GetComponent<AudioManager>();

        // find and set player reference
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerScript>();

        buttonFunctions.ButtonsInitialize();
        
        // Music for game
        //AudioManager.Instance.PlayMusic(playMusic);
        
    }

    void Update()
    {
        //delete this when done testing health buttons
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Pause Input
        if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Pause"))
        {
            if (!isPaused)
            {
                StatePause();
                buttonFunctions.BackgroundGroupOpen();
            }
            else
            {                
                StateUnPause();
            }
        }
    }

    // Game States //
    public void StatePause()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void StateUnPause()
    {
        isPaused = !isPaused;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        if(menuActive == buttonFunctions.SettingsMenu)
        {
            RectTransform settingsTransform = buttonFunctions.SettingsMenu.GetComponent<RectTransform>();

            settingsTransform.DOAnchorPos(new Vector3(-1983, 0, 0), 0.25f)
                             .SetEase(Ease.InOutQuad)
                             .SetUpdate(true);
            buttonFunctions.SettingsMenu.SetActive(false);
            menuActive = null;
        }
        buttonFunctions.BackgroundGroupClose();
    }
    //public void WinGame()
    //{
    //    StatePause();
    //    menuActive = menuWin;
    //    menuActive.SetActive(true);

    //}

    //public void LoseGame()
    //{
    //    StatePause();
    //    menuActive = menuLose;
    //    menuActive.SetActive(true);
    //}

}
