/*
    Author: Juan Contreras
    Edited By:
    Date Created: 01/16/2025
    Date Updated: 01/17/2025
    Description: Class to manage checkpoints the player sets off in game
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager instance;

    Vector3 lastCheckpointPosition;             //in case it becomes (0,0,0)

    public Vector3 LastCheckpointPosition => lastCheckpointPosition;

    // Awake is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetCheckpoint(Vector3 position)
    {
        if (SceneManagerScript.instance != null)
        {
            //store position of checkpoint
            //lastCheckpointPosition = position;

            SceneManagerScript.instance.SaveData.lastCheckpointPosition = position;

            //auto save game
            SceneManagerScript.instance.SaveGame();

            //Debug.Log($"Checkpoint set at: {SceneManagerScript.instance.SaveData.lastCheckpointPosition}");

            //event sent to player respawn
        }
    }    

    //public Vector3 GetCheckpointPosition()        //backup in case public getter does not work
    //{
    //    return lastCheckpointPosition;
    //}
}
