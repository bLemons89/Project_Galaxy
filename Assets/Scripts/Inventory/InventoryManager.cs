/*
    Author: Juan Contreras
    Date Created: 01/16/2025
    Date Updated: 01/16/2025
    Description: Everything related to the player's inventory starts here
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryManager : MonoBehaviour
{
    //singleton
    public static InventoryManager Instance;

    //Unity Event notifies Inventory was updated
    public UnityEvent OnInventoryUpdated;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); //keeps inventory between scenes
        }
        else
            Destroy(gameObject);
    }

    //inventory storage
    List<InventorySlot> inventorySlots = new List<InventorySlot>();

    //handles item pickup
    public void OnPickup(ItemBase item, int quantity)
    {
        //Debug.Log($"InventoryManager handling pickup: {item.ItemName}, Quantity: {quantity}");

        //add inventory limit maxSlots if statement return

        //check if item already exists
        InventorySlot existingSlot = inventorySlots.Find(slot => slot.Item == item);    //lambda expression, returns null if no match
        //check if item is stackable, if the item already exists
        if (existingSlot != null && item.MaxStackSize > 1)
        {
            //store number of that item that can fit in the current stack
            int availableSpace = item.MaxStackSize - existingSlot.Quantity;
            //store how many of the items collected to be stored in the current stack
            //i.e. if max stack = 10, player has 7 and collects 5, only 3 will be stored
            int addedQuantity = Mathf.Min(availableSpace, quantity);
            //adjust stack number
            existingSlot.Quantity += addedQuantity;

            //Debug.Log($"Existing Slot had {quantity} of {item.ItemName} added to it with a total of {existingSlot.Quantity} in the slot.");

            //adjust quantity for possible left overs
            quantity -= addedQuantity;
        }

        //new slot if no stack or if left over
        if(quantity > 0)
        {
            inventorySlots.Add(new InventorySlot(item, quantity));

            //Debug.Log($"New Slot created with item {item.ItemName} and quantity {quantity}");
        }

        //Debug.Log($"Added {quantity} of {item.ItemName} to inventory.");

        //update ui??

        //notifies that inventory was updated
        OnInventoryUpdated?.Invoke();   //Unity event (for other managers to listen for)
    }

    // Update is called once per frame
    void UpdateUI()
    {
        //event to update UI
        Debug.Log("Inventory UI updated.");
    }
}

public class InventorySlot
{
    //ItemBase = scriptable object for items (abstract class)
    public ItemBase Item { get; private set; }
    public int Quantity { get; set; }

    public InventorySlot(ItemBase item, int quantity)
    {
        Item = item;
        Quantity = quantity;
    }
}
