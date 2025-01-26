using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartDropoff : MonoBehaviour
{
    [Header("UI Elements")]
    //public TextMeshPro dropOffText;        //text element to display message
    [SerializeField] TextMeshPro partsCountText;     //text element to display the number of parts inserted

    [Header("References")]
    [SerializeField] InventoryManager playerInventory;    //reference to the player's InventoryManager

    int insertedParts = 0;              //tracks the number of parts inserted
    bool playerInRange = false;         //checks if the player is within the trigger

    private void Start()
    {
        //initialize the text display
        //dropOffText.text = "";
        UpdatePartsText();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))                       //ensure it's the player entering the trigger
        {
            playerInRange = true;
            //dropOffText.text = "[F] Insert Ship Part Parts:"; //show the drop-off message
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))                       //ensure it's the player leaving the trigger
        {
            playerInRange = false;
            //dropOffText.text = "";                            //hide the drop-off message
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))     //check if the player presses F
        {
            //try to drop off the collectible item
            DropOffCollectible();
        }
    }

    private void DropOffCollectible()
    {
        //check the player's inventory for a collectible item (ship part)
        InventorySlot collectibleSlot = playerInventory.InventorySlotsList.Find(slot => slot.Item.GetItemType == ItemBase.ItemType.Collectible);

        if (collectibleSlot != null)
        {
            //remove one collectible item from the inventory
            collectibleSlot.Quantity--;
            insertedParts++;

            //if the slot is empty, remove it from the inventory
            if (collectibleSlot.Quantity <= 0)
            {
                playerInventory.InventorySlotsList.Remove(collectibleSlot);
            }

            //update the UI
            UpdatePartsText();
            Debug.Log("Dropped off a collectible item");
        }
        else
        {
            Debug.Log("No collectible items in the player's inventory");
        }
    }

    private void UpdatePartsText()
    {
        //update the text showing the number of parts inserted
        partsCountText.text = $"{insertedParts}";
    }
}
