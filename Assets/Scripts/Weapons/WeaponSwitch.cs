using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponSwitch : MonoBehaviour
{
    [SerializeField] ItemBase items;
    // Start is called before the first frame update
    void Start()
    {
       


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            SwitchWeapon();
        }
    }

    private void SwitchWeapon()
    {

        InventorySlot inventorySlot = new InventorySlot(items, 1);

        if (inventorySlot != null)
            Debug.Log($"What is in player inventory:");          
        else {
            Debug.Log("No Inventory");
        }

    }
}
