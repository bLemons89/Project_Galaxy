/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/19/2025
    Date Updated: 01/19/2025
    Description: Core functionality of the stun enemy, takes an inventory item and runs away
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunEnemy : EnemyBase
{
    [Header("     Stun Enemy Stats     ")]
    [SerializeField] float stunDuration;
    //[SerializeField] private int enemyHP;
    [SerializeField] float enemySpeedMult;      //Speed multiplier
    [SerializeField] int fleeDistance = 2;

    //for interactions with the player
    GameObject player;
    playerScript playerSettings;
    enum EnemyState { Roaming, Chasing, Fleeing }            //Behavior changes when taking orb
    EnemyState currentState = EnemyState.Chasing;   //Starts by chasing the player

    bool isFleeing = false;     //Used to set speed once
    bool enemyHasItem;

    // Start is called before the first frame update
    void Start()
    {
        //Initializing stats
        //currentHealth = enemyHP;
        agent.speed *= speed;
        if (GameManager.instance != null)
        {
            player = GameManager.instance.Player;
            playerSettings = GameManager.instance.PlayerScript;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Behavior();     //the way the enemy acts around the player
    }

    void StunEnemyTakeDamage(float amount)
    {
        //drop orb right before dying
        if (currentHealth - amount <= 0)
        {
            //drop item logic
        }
        //call damage method (handles death)
        Debug.Log($"Stun Enemy: Took {amount} damage");
        //use unity event here
    }

    protected override void Behavior()
    {
        switch (currentState)
        {
            case EnemyState.Roaming:
                //Roam();s
                break;
            case EnemyState.Chasing:
                ChasePlayer();
                break;
            case EnemyState.Fleeing:
                FleePlayer();
                break;
        }
    }

    void ChasePlayer()
    {
        bool isInventoryEmpty = true;

        //checks if the player's inventory is empty
        if (InventoryManager.instance != null)
        {
            if (InventoryManager.instance.InventorySlotsList.Count > 0)
                isInventoryEmpty = false;
        }
        else
            Debug.Log("Stun Enemy: No Inventory Manager Instance");

        //will only go after player if inventory is not empty
        if (!isInventoryEmpty)
        {
            //move to player location anywhere on the scene when the player is within range
            agent.SetDestination(player.transform.position);
            //stun and take item from player
            if (Vector3.Distance(transform.position, player.transform.position) < agent.stoppingDistance)
            {
                StunPlayer();               //stuns the player
                TakeItemFromPlayer();        //takes item and flees
            }
        }
    }

    void FleePlayer()
    {
        //enemy runs faster
        if (!isFleeing)
        {
            agent.speed = (playerSettings.Speed *
                playerSettings.SprintMod) - 1;       //He runs slightly slower than player sprint speed

            //Prevent speed from being set more than once
            isFleeing = true;
        }

        //find direction away from player
        Vector3 playerPosition = player.transform.position;
        Vector3 fleeDirection = (transform.position - playerPosition).normalized;

        //destination to run to
        Vector3 fleeDestination = transform.position + fleeDirection * agent.stoppingDistance * fleeDistance;       //don't need agent.stoppingDistance??

        //move to that destination
        agent.SetDestination(fleeDestination);
    }

    private void StunPlayer()
    {
        CharacterController player = playerSettings.GetComponent<CharacterController>();
        //stun player for set duration
        //player.stun(stunDuration);        //stun status effect method here
    }

    private void TakeItemFromPlayer()
    {
        //accessing inventory manager
        foreach (var item in InventoryManager.instance.InventorySlotsList)
        {

            //taking a random item from player
            //generate random index to take item from slot

            //takes orb and attaches to the enemy
            //orb.takeOrb(transform);    //have a way for the enemy to hold the inventory item (remove item from inventory too)
            currentState = EnemyState.Fleeing;  //Change enemy state when taking the orb
            enemyHasItem = true;
            //UI changes?
            //GameManager.instance.toggleImage(false);
            break;

        }
    }
}
