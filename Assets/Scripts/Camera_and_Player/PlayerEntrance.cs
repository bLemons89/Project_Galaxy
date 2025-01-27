using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

// Put this script on the PlayerEntrance to spawn the player into the location here

public class PlayerEntrance : MonoBehaviour
{
    public enum Location
    {
        BETA_ShipHub,
        BETA_Outer_Ship_Area,
        BETA_Area1Platforms,
        BETA_Area2Industrial,
    }

    public Location currentLocation;

    void Start()
    {
        // In the begining of the game we need to put the player in the center of the ship   
        
        // else player coming from outside the ship, will use the PlayerEntrance object transform position
        CharacterController playerEntranceLocation = GameObject.FindWithTag("Player").GetComponent<CharacterController>();

        // assigning the PlayerEntrance object position to the Player.transform.position
        playerEntranceLocation.transform.position = transform.position;

        switch (currentLocation)
        {
            case Location.BETA_ShipHub:
                ExitFromBETA_Area2Industrial();
                break;


        }
    }

    public void ExitFromBETA_Area2Industrial()
    {        
        CharacterController playerEntranceLocation = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
        // assigning the PlayerEntrance object position to the Player.transform.position
        playerEntranceLocation.transform.position = transform.position;
    }

}
