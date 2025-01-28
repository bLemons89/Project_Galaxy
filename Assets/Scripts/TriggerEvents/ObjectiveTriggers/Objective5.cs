/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/28/2025
    Date Updated: 01/28/2025
    Description: Triggers the next mission in the queue if the criteria is met
                 (place on top of cell with other trigger(6))
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective5 : MonoBehaviour
{
    bool playerInRange;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Q) && InventoryManager.instance.MissionItemsCollected == 1)
        {
            GameManager.instance.GetComponent<ObjectiveManager>().CompleteObjective();

            //find all objects with the Objective4 script
            Objective5[] objectives = FindObjectsOfType<Objective5>();

            //loop through each object and destroy its GameObject
            foreach (Objective5 objective in objectives)
            {
                Destroy(objective.gameObject);
            }
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
