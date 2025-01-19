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

    enum EnemyState { Chasing, Fleeing }            //Behavior changes when taking orb
    EnemyState currentState = EnemyState.Chasing;   //Starts by chasing the player

    bool isFleeing = false;     //Used to set speed once
    bool enemyHasItem;

    // Start is called before the first frame update
    void Start()
    {
        //Initializing stats
        //currentHealth = enemyHP;
        agent.speed *= speed;
    }

    // Update is called once per frame
    void Update()
    {
        Behavior();     //the way the enemy acts around the player
    }

    public void StunEnemyTakeDamage(float amount)
    {
        //drop orb right before dying
        if (currentHealth - amount <= 0)
        {
            if (GameManager.instance.OrbScripts != null)
            {
                foreach (orbManager orb in GameManager.instance.OrbScripts)
                {
                    if (enemyHasItem && orb.transform.parent == transform)
                    {
                        orb.DropOrb(transform);     //drop orb at location of the parent
                    }
                }
            }
        }
        //calling base method for damage handling
        Debug.Log($"Stun Enemy: Took {amount} damage");
        //use unity event here
    }

    protected override void Behavior()
    {
        switch (currentState)
        {
            case EnemyState.Chasing:
                chasePlayer();
                break;
            case EnemyState.Fleeing:
                fleePlayer();
                break;
        }
    }

    private void chasePlayer()
    {
        bool playerHasOrb = false;
        //checks if the player is holding an orb
        if (GameManager.instance.OrbScripts != null)
        {
            foreach (orbManager orb in GameManager.instance.OrbScripts)
            {
                if (orb.IsHoldingOrb)
                {
                    playerHasOrb = true;
                    break;
                }
            }
        }

        if (playerHasOrb)
        {
            //move to player location anywhere on the scene when the player has the orb
            agent.SetDestination(GameManager.instance.Player.transform.position);
            //stun and take orb from player
            if (Vector3.Distance(transform.position, GameManager.instance.Player.transform.position) < agent.stoppingDistance)
            {
                stunPlayer();               //stuns the player
                takeOrbFromPlayer();        //takes orb and flees
            }
        }
    }

    private void fleePlayer()
    {
        //enemy runs faster
        if (!isFleeing)
        {
            agent.speed = (GameManager.instance.PlayerScript.Speed *
                GameManager.instance.PlayerScript.SprintMod) - 1;       //He runs slightly slower than player sprint speed
            //Prevent speed from being set more than once
            isFleeing = true;
        }

        //find direction away from player
        Vector3 playerPosition = GameManager.instance.Player.transform.position;
        Vector3 fleeDirection = (transform.position - playerPosition).normalized;

        //destination to run to
        Vector3 fleeDestination = transform.position + fleeDirection * agent.stoppingDistance * fleeDistance;

        //move to that destination
        agent.SetDestination(fleeDestination);
    }

    private void stunPlayer()
    {
        playerController player = GameManager.instance.Player.GetComponent<playerController>();
        //stun player for set duration if holding an orb
        foreach (orbManager orb in GameManager.instance.OrbScripts)
        {
            if (orb.IsHoldingOrb)
            {
                player.stun(stunDuration);
                break;
            }
        }
    }

    private void takeOrbFromPlayer()
    {
        //accessing orb manager
        foreach (orbManager orb in GameManager.instance.OrbScripts)
        {

            //taking orb from player
            if (orb.IsHoldingOrb)
            {
                //takes orb and attaches to the enemy
                orb.takeOrb(transform);    //Passing enemy transform
                currentState = EnemyState.Fleeing;  //Change enemy state when taking the orb
                enemyHasItem = true;
                //turn off orb UI icon
                GameManager.instance.toggleImage(false);
                break;
            }
        }
    }
