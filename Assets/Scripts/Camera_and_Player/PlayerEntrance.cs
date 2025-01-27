using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

// Put this script on the PlayerEntrance to spawn the player into the location here

public class PlayerEntrance : MonoBehaviour
{
    public enum LocationSpawnID
    {
        BETA_ShipHubInsideEntrance,
        BETA_Outer_Ship_AreaEntranceExitFromShipHub,
        BETA_Outer_Ship_AreaEntranceExitFromArea1PlatformsEntrance,
        BETA_Outer_Ship_AreaEntranceExitFromArea2IndustrialEntrance,
    }

    [SerializeField] private string sceneToLoad;
    [SerializeField] private LocationSpawnID currentLocationID;

    // Keep track of the last exit/entrance
    public static string LastPositionExitEntrance;
    CharacterController playerController;
    void Start()
    {
        switch (currentLocationID)
        {
            case LocationSpawnID.BETA_ShipHubInsideEntrance:
                
                break;
            case LocationSpawnID.BETA_Outer_Ship_AreaEntranceExitFromShipHub:
                playerController = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
                playerController.transform.position = new Vector3(31, 99, 81);
                break;
            case LocationSpawnID.BETA_Outer_Ship_AreaEntranceExitFromArea1PlatformsEntrance:
                playerController = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
                playerController.transform.position = new Vector3(89, 100, -94);
                break;
            case LocationSpawnID.BETA_Outer_Ship_AreaEntranceExitFromArea2IndustrialEntrance:
                playerController = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
                playerController.transform.position = new Vector3(177, 100, 56);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LastPositionExitEntrance = currentLocationID.ToString();
        }

    }



}
