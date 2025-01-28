/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/28/2025
    Date Updated: 01/28/2025
    Description: Triggers the next mission in the queue if the criteria is met
 */
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Objective2 : MonoBehaviour
{
    [SerializeField] GameObject firstCell;

    bool playerInRange;
    int enemiesAlive;

    private void Update()
    {
        if (playerInRange && enemiesAlive == 0)
        {
            GameManager.instance.GetComponent<ObjectiveManager>().CompleteObjective();
            firstCell.SetActive(true);

            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }

        if(other.GetComponent<EnemyBase>() != null)
            enemiesAlive++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }

        if (other.GetComponent<EnemyBase>() != null)
            enemiesAlive--;
    }
}
