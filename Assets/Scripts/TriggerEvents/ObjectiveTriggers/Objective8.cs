/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/28/2025
    Date Updated: 01/28/2025
    Description: Triggers the next mission in the queue if the criteria is met
 */
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

public class Objective8 : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI winText;

    bool playerInRange;
    GameObject boss;

    private void Update()
    {
        /*if (boss = GameObject.FindWithTag("Boss"))
        {

            if (playerInRange &&
                boss.GetComponent<HealthSystem>().CurrentHealth <= 0)
            {
                GameManager.instance.GetComponent<ObjectiveManager>().CompleteObjective();

                Destroy(gameObject);
            }
        }*/

        if (playerInRange && boss == null && InventoryManager.instance.MissionItemsCollected >= 3)
        {
            Time.timeScale = 0;
            winText.enabled = true;
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
