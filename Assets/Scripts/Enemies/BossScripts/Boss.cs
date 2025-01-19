/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/17/2025
    Date Updated: 01/18/2025
    Description: Logic for how the boss interacts with the player depending on the encounter.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Unity.VisualScripting;

public class Boss : MonoBehaviour
{
    int currentEncounter = 1;
    List<IBossAbility> abilities = new List<IBossAbility>();

    Transform player;
    NavMeshAgent agent;

    [Header("MOVEMENT SETTINGS")]
    [SerializeField] [Range(1, 20)] int moveSpeed = 3;         //walking speed of the boss
    [SerializeField] [Range(15, 50)] int chargeSpeed = 20;       //speed when charging at the player
    [SerializeField] [Range(5, 20)] int keepDistance = 10;     //distance to keep from the player       (use stopping distance?)
    [SerializeField] [Range(1, 30)] int chargeCooldown = 10;   //time between charges
    [SerializeField] [Range(1, 5)] int chargeStopDistance = 2; //distance for the boss to stop at when charging

    bool isCharging;
    bool isOnCooldown;
    Coroutine movementRoutine;

    public Transform Player => player;

    //Unity Events to notify when each ability is activated for animation and sound

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        agent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            Debug.LogError("Boss: Player not found.");
        }

        if (agent != null)
        {
            agent.speed = moveSpeed;
            SetupAbilities();           //double call in set next encounter method
            StartMovementBehavior();

            ActivateAbilities();
        }
    }

    void Update()
    {
        //if(!isCharging)
            //StartMovementBehavior();
    }

    public void AddAbility(IBossAbility ability)
    {
        abilities.Add(ability);
        ability.Initialize(this);
    }
    //the pattern in which the boss activates the abilities
    public void ActivateAbilities()
    {
        //logic to when to activate abilities (not done)
        foreach (IBossAbility ability in abilities)
        {
            ability.Execute();
        }
    }
    //called when boss is defeated
    public void SetupNextEncounter()
    {
        currentEncounter++;
        SetupAbilities();
    }

    public void SetupAbilities()
    {
        //setup in case there is time to add more abilities
        switch(currentEncounter)
        {
            case 1:
                FindAbility("ChargedLaser");
                FindAbility("GroundAttack");
                break;
            case 2:
                //AddAbility();
                //AddAbility();
                break;
            case 3:
                //AddAbility();
                //AddAbility();
                break;
        }
    }

    public void FindAbility(string abilityName)
    {
        Transform abilityTransform = transform.Find(abilityName);
        if (abilityTransform != null)
        {
            //iterate to find active abilities
            IBossAbility ability = abilityTransform.GetComponent<IBossAbility>();
            if (ability != null)
            {
                AddAbility(ability);
                abilityTransform.gameObject.SetActive(true);
            }
            else
                Debug.Log($"Ability {abilityName} is not found.");
        }
        else
            Debug.Log($"Ability transform for {abilityName} not found.");
    }

    public void StartMovementBehavior()
    {
        //calling the movement coroutine
        if(movementRoutine == null)
            movementRoutine = StartCoroutine(MovementBehavior());
    }

    IEnumerator MovementBehavior()
    {
        while (true)
        {
            if(!isCharging)
            {
                MaintainDistance();

                //charge after cooldown
                yield return new WaitForSeconds(chargeCooldown);

                //another check in case bool changes while on cooldown from other coroutine (might not need)
                if(!isCharging)
                    StartCoroutine(ChargeAtPlayer());
            }

            yield return null;
        }
    }

    public void MaintainDistance()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > keepDistance + 1f) //with adjustment for natural-like movement
        {
            //head to the player when far
            agent.isStopped = false;
            agent.SetDestination(player.position);
            Debug.Log("Boss: Moving closer to the player.");
        }
        else if (distanceToPlayer < keepDistance - 1f)
        {
            Debug.Log("Boss: Moving away from player");

            agent.isStopped = false;
            //move away to keep distance
            Vector3 directionAway = (transform.position - player.position).normalized;
            Vector3 destination = transform.position + directionAway * keepDistance;
            agent.SetDestination(destination);
        }
    }

    IEnumerator ChargeAtPlayer()
    {
        //set charging status and speed
        isCharging = true;
        agent.speed = chargeSpeed;
        //run towards the player
        Vector3 targetPosition = player.position;
        agent.SetDestination(targetPosition);

        while (Vector3.Distance(transform.position, player.position) > chargeStopDistance)
        {
            Debug.Log("Boss: Charging at player");
            //keep updating position to chase even if player is moving
            targetPosition = player.position;
            agent.SetDestination(targetPosition);
            //wait for next frame
            yield return null;
        }

        //start ground attack once close
        StartGroundAttack();

        //reset
        isCharging = false;
        agent.speed = moveSpeed;
    }

    public void StartGroundAttack()
    {
        Debug.Log("Boss: Starting GroundAttack");

        //trigger ground attack from list of abilities
        foreach(IBossAbility ability in abilities)
        {
            if(ability.GetType().Name == "GroundAttack")
            {
                ability.Execute();
                break;
            }
        }
    }
}
