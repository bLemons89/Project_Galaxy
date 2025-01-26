/*
    Author: Juan Contreras
    Edited By
    Date Created: 01/16/2025
    Date Updated: 01/19/2025
    Description: Everything related to the player's inventory starts here

    Possible Edits: 
        - Add Sorting or separate lists by type (for UI)
        - Dropping or removing item from inventory (button press or from UI)
 */
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class InventoryManager : MonoBehaviour
{
    //singleton
    public static InventoryManager instance;

    GameObject player;

    //Unity Event notifies Inventory was updated
    public UnityEvent OnInventoryUpdated;   //connect to CheckAvailable weapons in WeaponInAction

    //InventorySlot currentHeldItem;
    

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); //keeps inventory between scenes
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if(player == null)
            player = GameObject.FindWithTag("Player");
    }

    //inventory storage
    List<InventorySlot> inventorySlots = new List<InventorySlot>();

    public List<InventorySlot> InventorySlotsList => inventorySlots;

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

        //adjust weapon ammo if already in inventory
        if (existingSlot != null && item.GetItemType == ItemBase.ItemType.Weapon)
        {
            //add the ammo only
            WeaponInformation weapon = (WeaponInformation)existingSlot.Item;

            weapon.ammoStored += weapon.maxClipAmmo;
        }
        else if(quantity > 0)                                                   //new slot if no stack or if left over and not weapon type
        {
            inventorySlots.Add(new InventorySlot(item, quantity));

            //Debug.Log($"New Slot created with item {item.ItemName} and quantity {quantity}");
        }

        //Debug.Log($"Added {quantity} of {item.ItemName} to inventory.");

        if (item.GetItemType == ItemBase.ItemType.Weapon)
        {
            WeaponInAction weaponsToUpdate = player.GetComponent<WeaponInAction>();

            if (weaponsToUpdate != null)
            {
                weaponsToUpdate.CheckAvailableWeapons();
            }
        }
        //update ui??
        //notifies that inventory was updated
        OnInventoryUpdated?.Invoke();   //Unity event (for other managers to listen for)
    }

    public void OnDrop(ItemBase item, int quantity)
    {
        //find the item slot
        InventorySlot slot = inventorySlots.Find(s => s.Item == item);  //lambda expression

        if (slot != null)
        {
            //logic for stackable items
            if (item.MaxStackSize > 1)
            {
                //adjust stack quantity
                slot.Quantity -= quantity;

                //remove slot if no more items in it
                if (slot.Quantity <= 0)
                {
                    inventorySlots.Remove(slot);
                }
            }
            else
                inventorySlots.Remove(slot);    //go straight to removing slot if not stackable

            //notify other systems that the inventory has been updated
            OnInventoryUpdated?.Invoke();   //Unity event (for other managers to listen for)
        }
        else
            Debug.Log($"Item: {item.ItemName} not found in inventory");
    }

    //called to update UI (for possible future use)
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
