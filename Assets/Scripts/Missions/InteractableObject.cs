using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InteractableObject : MonoBehaviour
{
    public string ItemName;

    public bool playerInRange;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange && SelectionManager.instance.onTarget)
        {
            if (!InventorySystem.Instance.CheckIfFull())
            {
                InventorySystem.Instance.InsertIntoInv(ItemName);
                Destroy(gameObject);
            }
        }
    }


    public string GetItemName()
    {
        return ItemName;
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