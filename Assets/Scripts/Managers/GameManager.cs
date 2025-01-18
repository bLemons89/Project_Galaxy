using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //singleton
    public static GameManager instance;

    [Header("===== MANAGERS =====")]

    [Header("===== OVERLAYS =====")]
    [SerializeField] public GameObject backgroundScreen;

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
    {
        get => isPaused;
        set => isPaused = value;
    }
    public GameObject PlayerDamageScreen
    {
        get => playerDamageScreen;
        set => playerDamageScreen = value;
    }


    void Awake()
    {
        instance = this;

        // set original values
        timeScaleOrig = Time.timeScale;

        // find and set player reference
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerScript>();

        // find and set other reference

        // Music for game
        //AudioManager.Instance.PlayMusic(playMusic);
        
    }


    void Update()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Pause Input
        if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Pause"))
        {
            if (menuActive == null)
            {
                StatePause();
                menuActive = backgroundScreen;
                menuActive.SetActive(true);
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
        menuActive.SetActive(false);
        menuActive = null;
    }
}
