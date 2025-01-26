using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI;

    public List<GameObject> slotList = new List<GameObject>();
    public List<string> itemList = new List<string>();
    int stackSize;

    GameObject itemToAdd;
    GameObject whichSlot;

    public bool isOpen;
    // public bool isFull;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    void Start()
    {
        isOpen = false;
        PopulateSlots();
    }

    private void PopulateSlots()
    {
        foreach (Transform child in inventoryScreenUI.transform)
        {
            if (child.CompareTag("InvSlot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I) && !isOpen)
        {

            Debug.Log("i is pressed");
            inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            isOpen = false;
        }
    }

    public void InsertIntoInv(string itemName)
    {
        whichSlot = FindNextSlot();
        itemToAdd = (GameObject)Instantiate(Resources.Load<GameObject>(itemName), whichSlot.transform.position, whichSlot.transform.rotation);
        itemToAdd.transform.SetParent(whichSlot.transform);
        itemToAdd.name = itemName;
        itemList.Add(itemName);

    }

    private GameObject FindNextSlot()
    {
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount == 0)
                return slot;
        }
        return new GameObject();
    }

    public bool CheckIfFull()
    {
        int counter = 0;
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                counter++;
            }
        }
        if (counter == 21)
        {
            return true;
        }
        else return false;
    }

    public void RemoveItem(string itemToRemove, int amountToRemove)
    {
        // Count the occurrences of the item in the inventory
        int availableCount = itemList.Count(item => item == itemToRemove);

        if (availableCount < amountToRemove)
        {
            Debug.LogWarning($"Not enough items of type '{itemToRemove}' to remove. {amountToRemove - availableCount} items were missing.");
            return;
        }

        int counter = amountToRemove;

        for (var i = slotList.Count - 1; i >= 0 && counter > 0; i--)
        {
            if (slotList[i].transform.childCount > 0)
            {
                Transform child = slotList[i].transform.GetChild(0);

                // Match the item in slot with the target item
                if (child.name == itemToRemove)
                {
                    Destroy(child.gameObject);
                    counter--;

                    // Update the itemList to reflect the removed item
                    itemList.Remove(itemToRemove);
                }
            }
        }
    }

    public void ReCalculateList()
    {

        itemList.Clear();

        foreach (var item in slotList)
        {
            if (item.transform.childCount > 0)
            {
                string name = item.transform.GetChild(0).name;

                string str2 = ("Clone");

                string result = name.Replace(str2, "");

                itemList.Add(result);
            }
        }
    }
}