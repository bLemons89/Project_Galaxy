using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject inventorySlotPrefab;  // Prefab for UI slots
    public Transform inventoryGrid;  // The Grid Layout container

    private List<GameObject> slotInstances = new List<GameObject>();  // Stores active UI slots

    private void OnEnable()
    {
        // Subscribe to Inventory Updates
        if (InventoryManager.instance != null)
            InventoryManager.instance.OnInventoryUpdated.AddListener(UpdateInventoryUI);
    }

    private void OnDisable()
    {
        if (InventoryManager.instance != null)
            InventoryManager.instance.OnInventoryUpdated.RemoveListener(UpdateInventoryUI);
    }

    public void UpdateInventoryUI()
    {
        // Clear old slots
        foreach (GameObject slot in slotInstances)
        {
            Destroy(slot);
        }
        slotInstances.Clear();

        // Populate with current inventory
        foreach (InventorySlot slot in InventoryManager.instance.InventorySlotsList)
        {
            GameObject newSlot = Instantiate(inventorySlotPrefab, inventoryGrid);
            slotInstances.Add(newSlot);

            // Assign UI elements
            Image icon = newSlot.transform.Find("ItemIcon").GetComponent<Image>();
            TMP_Text itemName = newSlot.transform.Find("ItemName").GetComponent<TMP_Text>();
            TMP_Text quantityText = newSlot.transform.Find("ItemAmount").GetComponent<TMP_Text>();

            // Set data
            icon.sprite = slot.Item.Icon;
            itemName.text = slot.Item.ItemName;
            quantityText.text = $"x{slot.Quantity} / {slot.Item.MaxStackSize}";
        }
    }
}
