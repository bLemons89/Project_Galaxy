/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/28/2025
    Date Updated: 02/01/2025
    Description: Triggers the next mission in the queue if the criteria is met
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective0 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") &&
            InventoryManager.instance.InventorySlotsList.Count > 0)     //change to weapon specific?
        {
            ObjectiveManager.instance.CompleteObjective();

            Destroy(gameObject);        //remove trigger once complete
        }
    }
}
