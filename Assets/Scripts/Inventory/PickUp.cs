/*
    Author: Breanna Lemons
    Edited By: Juan Contreras
    Date Created: 01/16/2025
    Date Updated: 01/16/2025
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
    public UnityEvent<ItemBase, int> OnPickup;

    void OnTriggerEnter(Collider other)
    {
        //prevent other triggers from triggering??

        //check for player collider
        if(other.CompareTag("Player"))
        {
            //trigger unity event to notify inventory manager
            OnPickup?.Invoke(item, quantity);

            //destroy item in the world
            Destroy(gameObject);
        }
    }
}
