/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/28/2025
    Date Updated: 01/28/2025
    Description: Triggers the next mission in the queue if the criteria is met
                 (put outside ship)
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective1 : MonoBehaviour
{
    bool playerInRange;

    private void Update()
    {
        if (playerInRange)
        {
            GameManager.instance.GetComponent<ObjectiveManager>().CompleteObjective();

            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
