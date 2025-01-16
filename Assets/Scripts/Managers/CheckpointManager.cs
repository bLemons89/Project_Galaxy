using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        //activate checkpoint
    }    

    public void GetCheckpointPosition()
    {

    }
}
