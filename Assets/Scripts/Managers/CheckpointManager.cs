/*
    Author: Juan Contreras
    Edited By:
    Date Created: 01/16/2025
    Date Updated: 01/16/2025
    Description: Class to manage checkpoints the player sets off in game
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager instance;

    Vector3 checkpointPosition;
    bool activeCheckpoint;

    public bool ActiveCheckpoint => activeCheckpoint;       //true if there is a current checkpoint saved

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
        //store position of checkpoint
        checkpointPosition = position;
        //activate checkpoint
        activeCheckpoint = true;

        Debug.Log($"Checkpoint set at: {checkpointPosition}");
    }    

    public Vector3 GetCheckpointPosition()         //may not need as long as there is always a checkpoint
    {
        if (activeCheckpoint)
        {
            return checkpointPosition;
        }
        else
            return Vector3.zero;
    }
}
