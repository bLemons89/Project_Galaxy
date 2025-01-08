using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] WeaponInformation gunInfo;

    // Start is called before the first frame update
    public int GetAmmo()
    {
        return gunInfo.currentAmmo;
    }

    public void SetAmmo(int newAmmo)
    {
        gunInfo.currentAmmo = newAmmo;
    }

    public void Shoot()
    {
        gunInfo.currentAmmo--;
    }

    public void Reload()
    {
        gunInfo.currentAmmo = gunInfo.maxAmmo;
    }

}
