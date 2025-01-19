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
    //scene manager
    //audio manager

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

    void Awake()
    {
        instance = this;
        buttonFunctions = FindObjectOfType<ButtonFunctions>();

        // set original values
        timeScaleOrig = Time.timeScale;

        // find and set player reference
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerScript>();

        buttonFunctions.ButtonsInitialize();
        
        // find and set other reference

        // Music for game
        //AudioManager.Instance.PlayMusic(playMusic);
        
    }


    void Update()
    {
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
