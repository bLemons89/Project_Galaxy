using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    
    [SerializeField] WeaponInformation gunInfo;

    private void Start()
    {
        PlayerShoot.shootInput += PlayerShoot_shootInput;
        PlayerShoot.weaponReload += Reload;
    }    

    // Example from Unity: Draws a 10 meter long green line from the position for 1 frame.
    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(transform.position, forward, Color.green);
    }


    private void PlayerShoot_shootInput()
    {
        if(gunInfo.currentAmmo > 0)
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, gunInfo.shootDistance))
            {
                Debug.Log(hitInfo.transform.name);
                gunInfo.currentAmmo--;
            }
            
        }
        else
        {
            // Inform the player running out of ammo using Text or HUD image to
            // inform the player to press "r" to reload the ammo
            #if UNITY_EDITOR
                Debug.Log("No More Ammo, Press r to Reload!!");
            #endif
        }
        
    }

    public int GetAmmo()
    {
        return gunInfo.currentAmmo;
    }

    public void SetAmmo(int newAmmo)
    {
        gunInfo.currentAmmo = newAmmo;
    }

    public void Reload()
    {
        #if UNITY_EDITOR
            Debug.Log($"Reloading ammo: {gunInfo.currentAmmo}");
        #endif

        gunInfo.currentAmmo = gunInfo.maxAmmo;
    }

    public int GetShootDamage()
    {
        return gunInfo.shootDamage;
    }
}
