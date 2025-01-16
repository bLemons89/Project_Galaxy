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
            Debug.Log("No More Ammo, Press r to Reload!!");
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
        Debug.Log($"Reloading ammo: {gunInfo.currentAmmo}");
        gunInfo.currentAmmo = gunInfo.maxAmmo;
    }

}
