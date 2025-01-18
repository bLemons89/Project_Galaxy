using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //singleton
    public static GameManager instance;
    ButtonFunctions buttonFunctions;

    [Header("===== MANAGERS =====")]

    [Header("===== OVERLAYS =====")]
    [SerializeField] public GameObject backgroundScreen;
    [SerializeField] public GameObject settingsCanvasGroup;
    private CanvasGroup backgroundScreenGroup;
    private CanvasGroup settingsGroup;

    [Header("===== PLAYER =====")]
    private GameObject player;
    private playerScript playerScript;
    [SerializeField] GameObject playerDamageScreen;

    [Header("===== TEMP VARIABLES =====")]
    [SerializeField] GameObject menuActive;

    // Flags //
    private bool isPaused;

    // Cache //
    float timeScaleOrig;

    // Getters and Setters //
    public bool IsPaused
    {
        get => isPaused;
        set => isPaused = value;
    }
    public GameObject PlayerDamageScreen
    {
        get => playerDamageScreen;
        set => playerDamageScreen = value;
    }
    public GameObject BackgroundScreen
    {
        get => backgroundScreen;
        set => backgroundScreen = value; 
    }
    public CanvasGroup BackgroundScreenGroup
    {
        get => backgroundScreenGroup;
        set => backgroundScreenGroup = value;
    }
    public CanvasGroup SettingsGroup
    {
        get => settingsGroup; 
        set => settingsGroup = value;
    }

    void Awake()
    {
        instance = this;
        buttonFunctions = FindObjectOfType<ButtonFunctions>();

        // set original values
        timeScaleOrig = Time.timeScale;

        // find and set player reference
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerScript>();

        backgroundScreenGroup = backgroundScreen.GetComponent<CanvasGroup>();
        backgroundScreenGroup.alpha = 0f;
        backgroundScreen.transform.localScale = Vector3.zero;
        settingsGroup = settingsCanvasGroup.GetComponent<CanvasGroup>();
    }

   
    void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        // Pause Input
        if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Pause"))
        {
            if (menuActive == null)
            {
                StatePause();
                menuActive = backgroundScreen;
                menuActive.SetActive(true);

                buttonFunctions.BackgroundGroupOpen();
            }
            else if (menuActive == backgroundScreen)
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

        buttonFunctions.BackgroundGroupClose();

        menuActive = null;        
    }
}
