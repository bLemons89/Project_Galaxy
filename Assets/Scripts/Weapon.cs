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
    }

    private void PlayerShoot_shootInput()
    {
        gunInfo.currentAmmo--;
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
        gunInfo.currentAmmo = gunInfo.maxAmmo;
    }

}
