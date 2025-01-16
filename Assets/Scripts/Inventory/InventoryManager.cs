/*
    Author: Juan Contreras
    Date Created: 01/16/2025
    Date Updated: 01/16/2025
    Description: Everything related to the player's inventory starts here
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    //singleton
    public static InventoryManager Instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    //inventory storage
    List<InventorySlot> inventorySlots = new List<InventorySlot>();

    // Update is called once per frame
    void Update()
    {
        
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
