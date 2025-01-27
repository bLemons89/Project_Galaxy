using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountNDestroy : MonoBehaviour
{
    bool playerInRange;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Q))
        {
            if(InventoryManager.instance != null)
            {
                InventoryManager.instance.MissionItemsCollected++;

                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))                       //ensure it's the player entering the trigger
        {
            playerInRange = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))                       //ensure it's the player leaving the trigger
        {
            playerInRange = false;
        }
    }
}
