using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : EnemyBase
{
    [Header("     Ranged Enemy Stats     ")]
    [SerializeField] private WeaponInformation equippedWeapon;
    [SerializeField] private Transform gunTransform; // Where the gun is attached
    [SerializeField] private Transform shootPoint; // Where bullets originate from    
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int FOV;
    [SerializeField] int roamDist;  // sphere distance of roaming
    [SerializeField] int roamTimer; // how long to wait before move again

    public static event Action OnShootingPlayer;    

    private GameObject player;
    private bool isRoaming = false;
    private float shootRate;
    private int currentAmmo;
    private int maxAmmo;
    private int shootDistance;
    private int reloadRate;
    Vector3 startingPos;
    private bool isShooting = false;

    void Start()
    {
        currentAmmo = equippedWeapon.maxAmmo;
        shootDistance = equippedWeapon.shootDistance;
        maxAmmo = equippedWeapon.maxAmmo;
        shootRate = equippedWeapon.shootRate;

        player = GameManager.instance.Player; // Assume GameManager handles player reference
        startingPos = transform.position; // to remember the starting position for roaming

        if (equippedWeapon == null)
        {
            Debug.LogError("RangedEnemy: No weapon equipped!");
        }

        StartCoroutine(roam());
    }

    void Update()
    {
        Behavior();
    }

    protected override void Behavior()
    {
        if (player == null) return;

        // Aim at the player
        AimAtPlayer();

        // Check if the enemy can shoot
        if (CanShootPlayer())
        {
            ShootAtPlayer();
        }
    }

    private void AimAtPlayer()
    {
        // Calculate direction to the player
        Vector3 direction = (player.transform.position - transform.position).normalized;

        // Rotate the gun to face the player
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(gunTransform.rotation, lookRotation, Time.deltaTime * 5f);
        
    }

    private bool CanShootPlayer()
    {
        // Check if the player is within shooting distance
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        return distanceToPlayer <= shootDistance;
    }

    private void ShootAtPlayer()
    {      

        // Check if the weapon has ammo
        if (currentAmmo > 0)
        {
            // if not shooting, begin shooting
            if (!isShooting)
            {
                StartCoroutine(shoot());
            }
            
        }
        else
        {
            Debug.Log("RangedEnemy: Out of ammo, reloading...");
            StartCoroutine(ReloadWeapon());
        }
    }

    private System.Collections.IEnumerator ReloadWeapon()
    {
        yield return new WaitForSeconds(reloadRate);
        currentAmmo = maxAmmo;
        Debug.Log("RangedEnemy: Reload complete.");
    }


    // Enemy Roaming //
    IEnumerator roam()
    {
        // turn on 
        isRoaming = true;

        // IEnums must have yield
        yield return new WaitForSeconds(roamTimer); // wait for second before continuing. 

        // only for roaming to make sure the AI reaches destination
        agent.stoppingDistance = 0;

        // how big is our roaming distance 
        Vector3 randomPos = UnityEngine.Random.insideUnitSphere * roamDist;
        randomPos += startingPos;

        // Enemy is Hit by Player //
        NavMeshHit hit; // get info using similar like raycast
        NavMesh.SamplePosition(randomPos, out hit, roamDist, 1); // remember where the hit is at. 
        agent.SetDestination(hit.position); // player last known position

        // turn off
        isRoaming = false;
    }

    IEnumerator shoot()
    {
        // turn on
        isShooting = true;
               
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, shootDistance))
        {
            OnShootingPlayer?.Invoke();
            Debug.Log(hitInfo.transform.name + $" Got Hit");
            --currentAmmo;
        }
        
        // enemySpeedMult
        yield return new WaitForSeconds(shootRate);

        // turn off
        isShooting = false;
    }


}
