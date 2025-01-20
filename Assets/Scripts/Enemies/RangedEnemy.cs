using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : EnemyBase
{
    [Header("     Ranged Enemy Stats     ")]
    [SerializeField] private WeaponInformation equippedWeapon; //weapon data (Scriptable Object)
    [SerializeField] private Transform gunTransform;           //where the gun is attached
    [SerializeField] private Transform shootPoint;             //where bullets originate from

    private GameObject player;

    void Start()
    {
        player = GameManager.instance.Player;

        if (equippedWeapon == null)
        {
            Debug.LogError("RangedEnemy: No weapon equipped!");
        }
    }

    void Update()
    {
        Behavior();
    }

    protected override void Behavior()
    {
        if (player == null) return;

        //aim at the player
        AimAtPlayer();

        //check if the enemy can shoot
        if (CanShootPlayer())
        {
            ShootAtPlayer();
        }
    }

    private void AimAtPlayer()
    {
        //calculate direction to the player
        Vector3 direction = (player.transform.position - transform.position).normalized;

        //rotate the gun to face the player
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        gunTransform.rotation = Quaternion.Slerp(gunTransform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private bool CanShootPlayer()
    {
        //check if the player is within shooting distance
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        return distanceToPlayer <= equippedWeapon.shootDistance;
    }

    private void ShootAtPlayer()
    {
        //check if the weapon has ammo
        if (equippedWeapon.currentAmmo > 0)
        {
            // Call the gun's shooting script or logic
            Debug.Log("RangedEnemy: Shooting at the player.");

            //call shoot

            equippedWeapon.currentAmmo--;

            //deal damage here
        }
        else
        {
            Debug.Log("RangedEnemy: Out of ammo, reloading...");
            StartCoroutine(ReloadWeapon());
        }
    }

    private System.Collections.IEnumerator ReloadWeapon()
    {
        yield return new WaitForSeconds(equippedWeapon.reloadRate);
        equippedWeapon.currentAmmo = equippedWeapon.maxAmmo;
        Debug.Log("RangedEnemy: Reload complete.");
    }
}
