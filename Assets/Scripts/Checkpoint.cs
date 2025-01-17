/*
    Author: Juan Contreras
    Edited By:
    Date Created: 01/16/2025
    Date Updated: 01/16/2025
    Description: Class to use when setting checkpoints in a scene to respawn
                 at when player loses.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    public UnityEvent<Vector3> OnCheckpointActivated;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OnCheckpointActivated?.Invoke(transform.position);

            Debug.Log($"Checkpoint activated at {transform.position}");
        }
    }
}
