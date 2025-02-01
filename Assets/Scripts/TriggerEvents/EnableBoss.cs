/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/26/2025
    Date Updated: 01/27/2025
    Description: Trigger logic to enable the boss once the player has collected all the cells
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableBoss : MonoBehaviour
{
    [SerializeField] GameObject boss;

    /*private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && InventoryManager.instance.MissionItemsCollected >= 3)      //maybe link to another number, works for now
        {
            boss.SetActive(true);
        }
    }*/
}
