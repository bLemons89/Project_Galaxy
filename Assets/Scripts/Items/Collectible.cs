/*
    Author: Juan Contreras
    Date Created: 01/15/2025
    Date Updated: 01/15/2025
    Description: Abstract class to use when creating scriptable objects for collectibles to be
                 picked up in the game scene(s).
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCollectible", menuName = "Collectible")]        //for now, will most likely move to derived classes only
public class Collectible : ItemBase
{
    [SerializeField] protected int value;   //one could have higher value than others

    public override void IntendedUse()
    {

    }
}
