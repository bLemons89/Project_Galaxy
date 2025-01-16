/*
    Author: Juan Contreras
    Date Created: 01/15/2025
    Date Updated: 01/15/2025
    Description: Weapon class scriptable objects for the player to use and store in inventory
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapon")]
public class Weapon : ItemBase
{
    [SerializeField] private int damage;
    
    //read-only getter
    public int Damage => damage;

    public override void IntendedUse()
    {
        //equipping
    }

    public virtual void OnUse() { }
}
