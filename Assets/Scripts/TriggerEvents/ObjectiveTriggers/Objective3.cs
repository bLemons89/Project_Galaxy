/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/28/2025
    Date Updated: 02/01/2025
    Description: Triggers the next mission in the queue if the criteria is met
                 (Pick Up energy cell)
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective3 : MonoBehaviour
{
    [SerializeField] GameObject[] exitColliders;

    bool playerInRange;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Q))
        {
            InventoryManager.instance.CollectEnergyCell();
            //ObjectiveManager.instance.CompleteObjective();

            foreach (GameObject obj in exitColliders)           //turn off colliders to allow plater to continue
            {
                obj.SetActive(false);
            }

            Destroy(gameObject);      //remove from parent
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
