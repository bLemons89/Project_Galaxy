using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using System.Collections;
using TMPro;
using UnityEditor;


// Put this script on the object to trigger which scene to enter
public class SceneManagerScript : MonoBehaviour
{
    public static SceneManagerScript Instance;    

    [SerializeField] private string sceneToLoad;

    [Header("===== Display Timer Count Down =====")]
    [SerializeField] private TMP_Text TimerCountDown;

    private string playerTag = "Player";

    // this variable is use to reset the scene testing
    private int restartSceneTimer;
    private float counter;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        restartSceneTimer = 15;        
        
        // This Coroutine for testing rest the scene
        // StartCoroutine(DelayLoading());
    }

    private void Update()
    {
        TimerCountDown.text = counter.ToString("F0");
        counter += Time.deltaTime;

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

    // if player died the ScreenManagerScript needs to reset the scene, trigger the loadingScreen
    // respawn the player to the latest checked point

    public void ResetScene()
    {
        RestingData();
        // reset the location of the enemy position, reset part location, etc. 
        //Scene thisScene = SceneManager.GetActiveScene();
        //Debug.Log("Active Scene is '" + thisScene.name + "'.");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator DelayLoading()
    {
        yield return new WaitForSeconds(restartSceneTimer);
        ResetScene();
    }

    private void RestingData()
    {
        InventoryManager.Instance.InventorySlotsList.Clear(); 
    }
}
