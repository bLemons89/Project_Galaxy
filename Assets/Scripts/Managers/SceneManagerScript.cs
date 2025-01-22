using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using System.Collections;
using TMPro;
using UnityEditor;


// Put this script on the object to trigger which scene to enter
public class SceneManagerScript : MonoBehaviour
{
    public static SceneManagerScript instance;

    [SerializeField] private string sceneToLoad;

    [Header("===== Display Timer Count Down =====")]
    [SerializeField] private TMP_Text TimerCountDown;

    private string playerTag = "Player";

    private float counter;

    private void Awake()
    {
        instance = this;
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

    // Load Menu Scene (or scene index 0)
    // TODO: 
    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene(0);
    }


    private void RestingData()
    {
        InventoryManager.instance.InventorySlotsList.Clear(); 
    }
}
