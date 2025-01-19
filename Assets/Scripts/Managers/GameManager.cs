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

    [Header("===== MANAGERS =====")]
    private GameObject sceneManagerEmpty;
    private GameObject audioManagerEmpty;
    private SceneManagerScript sceneManager;
    private AudioManager audioManager;

    [Header("===== PLAYER =====")]
    private GameObject player;
    private playerScript playerScript;
    [SerializeField] GameObject playerDamageScreen;

    [Header("===== TEMP VARIABLES =====")]
    [SerializeField] GameObject menuActive;

    [Header("===== MUSIC =====")]
    //[SerializeField] private AudioClip playMusic;

    // Flags //
    private bool isPaused;

    // Cache //
    float timeScaleOrig;

    // Getters and Setters //
    public bool IsPaused
    {get => isPaused; set => isPaused = value;}
    public GameObject PlayerDamageScreen
    {get => playerDamageScreen; set => playerDamageScreen = value;}
    public playerScript PlayerScript
    {get => playerScript; set => playerScript = value;}

    void Awake()
    {
        instance = this;
        buttonFunctions = FindObjectOfType<ButtonFunctions>();

        // set original values
        timeScaleOrig = Time.timeScale;

        sceneManagerEmpty = GameObject.FindWithTag("Scene Manager");
        sceneManager = sceneManagerEmpty.GetComponent<SceneManagerScript>();

        audioManagerEmpty = GameObject.FindWithTag("Audio Manager");
        audioManager = audioManagerEmpty.GetComponent<AudioManager>();

        // find and set player reference
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerScript>();

        buttonFunctions.ButtonsInitialize();
        
        // Music for game
        //AudioManager.Instance.PlayMusic(playMusic);
        
    }

    void Update()
    {
        //delete this 
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
        
        buttonFunctions.BackgroundGroupClose();
    }
}
