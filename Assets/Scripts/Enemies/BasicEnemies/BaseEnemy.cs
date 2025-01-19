using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


public abstract class baseEnemy : MonoBehaviour, IDamage
{
    [Header("     Base Enemy Stats     ")]
    [SerializeField] protected NavMeshAgent agent;      //Components shared between all/most enemy types
    [SerializeField] protected Renderer model;
    [SerializeField] protected Animator animator;

    [SerializeField] protected Image enemyHPBar;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float currentHealth;

    [SerializeField] protected float fillSpeed;
    [SerializeField] protected Gradient colorGradient;


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
    public Image EnemyHPBar
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
    }

    public virtual void takeDamage(float amount)     
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
            Destroy(gameObject);       
    }

    
    protected abstract void Behavior();    
}