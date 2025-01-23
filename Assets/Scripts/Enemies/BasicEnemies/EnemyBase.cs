/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/19/2025
    Date Updated: 01/22/2025
    Description: Abstract class for all enemies
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("BASE ENEMY STATS")]
    [SerializeField] protected NavMeshAgent agent;      //Components shared between all/most enemy types
    [SerializeField] protected int speed;
    [SerializeField] protected Renderer model;
    [SerializeField] protected Animator animator;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float currentHealth;

    [Header("WEAPON AND TARGETING")]
    [SerializeField] protected WeaponInAction weaponInAction;     //weapon system for weapon in use
    [SerializeField] protected TargetingSystem targetingSystem;   //targeting system for enemies
    [SerializeField] int enemyShootRate;

    //[SerializeField] LayerMask ignoreMask;          //prevents from damaging each other
    //[SerializeField] protected Image enemyHPBar;


    //[SerializeField] protected float fillSpeed;
    //[SerializeField] protected Gradient colorGradient;


    protected Vector3 playerDirection;

    public float CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }
    public float MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }

    public int EnemyShootRate => enemyShootRate;
    /*public Image EnemyHPBar
    {
        get { return enemyHPBar; }
        set { enemyHPBar = value; }
    }
    public float FillSpeed
    {
        get { return fillSpeed; }
        set { fillSpeed = value; }
    }
    public Gradient ColorGradient
    {
        get { return colorGradient; }
        set { colorGradient = value; }
    }*/

    //To be defined in each enemy class
    protected abstract void Behavior();     //For consistency and clarity           //in update with HandleWeapon

    //handle weapon logic for the enemy holding the weapon (AI)
    protected void HandleWeapon()
    {
        //if(weaponInAction.GunInfo == null)
                //weaponInAction.EquipWeapon(0);

        targetingSystem.AimAtTarget();

        if (targetingSystem.CurrentTarget != null)
        {
            //look at target
            Vector3 direction = targetingSystem.CurrentTarget.position - transform.position;
            direction.y = 0;                                                                    //keeps turning horizontal only (might delete)
            transform.rotation = Quaternion.LookRotation(direction);

            //shoot while gun has ammo in the clip
            if (weaponInAction.CurrentAmmo > 0)
            {
                weaponInAction.FireGun();
            }
            else
            {
                weaponInAction.Reload();
            }
        }
    }

    public virtual void TakeDamage(float amount)      //All enemies take damage
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
            Destroy(gameObject);        //Dead
    }
}
