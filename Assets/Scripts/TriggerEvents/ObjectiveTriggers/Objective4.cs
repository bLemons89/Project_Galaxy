/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/28/2025
    Date Updated: 01/28/2025
    Description: Triggers the next mission in the queue if the criteria is met
                 (place in area: Destroy both when 1 is triggered)   
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective4 : MonoBehaviour
{
    bool playerInRange;

    private void Update()
    {
        if (playerInRange)
        {
            GameManager.instance.GetComponent<ObjectiveManager>().CompleteObjective();

            //find all objects with the Objective4 script
            Objective4[] objectives = FindObjectsOfType<Objective4>();

            //loop through each object and destroy its GameObject
            foreach (Objective4 objective in objectives)
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
