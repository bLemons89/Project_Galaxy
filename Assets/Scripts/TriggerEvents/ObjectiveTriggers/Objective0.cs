/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/28/2025
    Date Updated: 02/01/2025
    Description: Triggers the next mission in the queue if the criteria is met
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective0 : MonoBehaviour
{
    [SerializeField] GameObject exitCollider;

    string objectiveID = "0";

    private void OnTriggerEnter(Collider other)
    {
        if (!SceneManagerScript.instance.SaveData.IsObjectiveCompleted(objectiveID))
        {
            if (other.CompareTag("Player") &&
            InventoryManager.instance.InventorySlotsList.Find(slot => slot.Item.ItemType_ == ItemBase.ItemType.Weapon) != null)
            {
                ObjectiveManager.instance.CompleteObjective();

                //mark as complete
                SceneManagerScript.instance.SaveData.MarkObjectiveAsCompleted(objectiveID);
                SceneManagerScript.instance.SaveGame();     //save progress

                exitCollider.SetActive(false);

                Destroy(gameObject);        //remove trigger once complete
            }
        }
    }
}
