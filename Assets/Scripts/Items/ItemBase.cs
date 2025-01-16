/*
    Author: Juan Contreras
    Date Created: 01/15/2025
    Date Updated: 01/15/2025
    Description: Abstract class to use when creating scriptable objects for items to be
                 picked up in the game scene(s).
 */
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public abstract class ItemBase : ScriptableObject
{
    [SerializeField] protected string itemName;
    [SerializeField] protected Sprite icon;
    [SerializeField] protected ItemType itemType;
    [SerializeField] protected int maxStackSize = 1;
    
    public string ItemName => itemName;
    public int MaxStackSize => maxStackSize;

    public enum ItemType
    {
        Weapon,
        Ammo,
        Collectible,
    }

    public abstract void IntendedUse();
}
