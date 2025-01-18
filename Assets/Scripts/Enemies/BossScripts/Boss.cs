/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/17/2025
    Date Updated: 01/17/2025
    Description: Logic for how the boss interacts with the player depending on the encounter.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    int currentEncounter = 1;
    List<IBossAbility> abilities = new List<IBossAbility>();

    Transform player;

    public Transform Player => player;

    //Unity Events to notify when each ability is activated for animation and sound

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
                AddAbility(new ChargedLaser());
                AddAbility(new GroundAttack());
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

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogError("Boss: Player not found.");
        }
        else
            SetupAbilities();
    }
}
