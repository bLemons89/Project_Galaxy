/*
    Author: Breanna Lemons
    Edited By: Juan Contreras
    Date Created: 01/16/2025
    Date Updated: 01/25/2025
    Description: Class to use when picking up objects in a scene to be
                 stored in the inventory.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickUp : MonoBehaviour
{
    [Header("Scriptable Here")]
    [SerializeField] ItemBase item;
    [SerializeField] int quantity = 1;  //number of that item this pickup will add to the inventory

    //unity event to notify InventoryManager
    //public UnityEvent<ItemBase, int> OnPickup;
    //public UnityEvent OnWeaponPickup;

    void OnTriggerEnter(Collider other)         //add key press input check in if statement
    {
        //prevent other triggers from triggering??

        //check for player collider
        if(other.CompareTag("Player"))
        {
            if (item != null && InventoryManager.instance)
            {
                InventoryManager.instance.OnPickup(item, quantity);
                
                //if (item.GetItemType == ItemBase.ItemType.Weapon)       //not being used
                    //OnWeaponPickup?.Invoke();

                //trigger unity event to notify inventory manager   (loses reference if destroyed or instantiated)
                //OnPickup?.Invoke(item, quantity);
            }

            //destroy item in the world
            Destroy(gameObject);
        }
    }

}
