/*
    Author: Harry Tanama
    Edited by: Juan Contreras
    Date Created: 01/18/2025
    Date Updated: 01/23/2025
    Description: Script to handle all gun functionalities and store gun info from scriptables
                 **GUN CONTROLS, DOES NOT UPDATE**

    Dev Notes: Keep eye on ammo retention
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class WeaponInAction : MonoBehaviour
{
    [Header("WEAPON INFO")]
    [Header("Weapon Scriptable List")]
    [SerializeField] List<WeaponInformation> availableWeapons = new List<WeaponInformation>();   //player and enemy can use    (connect to player inventory)
    [SerializeField] GameObject gunModelPlaceHolder;
    [SerializeField] TMP_Text reloadText;
    [SerializeField] GameObject reloadMessage;

    //===========VARIABLES===========
    WeaponInformation gunInfo;
    //int currentWeaponIndex = 0;
    int currentAmmo = 0;
    int ammoStored = 0;

    bool isReloading;
    bool isShooting;
    bool isFlashing;
    //===========GETTERS===========
    public int CurrentAmmo => currentAmmo;

    public GameObject GunModelPlaceHolder => gunModelPlaceHolder;
    public WeaponInformation GunInfo { get; set; }

    private void Start()
    {
        if (this.gameObject.CompareTag("Player"))
        {
            CheckAvailableWeapons();

            EquipWeapon(0);
        }
    }

    private void Update()
    {
        if (this.gameObject.CompareTag("Player"))
        {
            OnSwitchWeapon();

            if (Input.GetKeyDown(KeyCode.Mouse0))
                FireGun();

            if (Input.GetKeyDown(KeyCode.R))
                Reload();
        }
    }
    public void OnSwitchWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && availableWeapons.Count > 0)        //press 1 for primary
        {
            EquipWeapon(0);

            if(reloadMessage.activeSelf)
                reloadMessage.SetActive(false);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && availableWeapons.Count > 0)
        {
            EquipWeapon(1);

            if (reloadMessage.activeSelf)
                reloadMessage.SetActive(false);
        }

        /* USE IF ADDING MORE EQUIPABLE WEAPONS
        for (int i = 0; i < availableWeapons.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                EquipWeapon(i);
                break;
            }*/
    }

    //PLAYER ONLY: updates weapons based on weapons in the inventory
    public void CheckAvailableWeapons()                 //called with Unity Event when updated
    {
        if (InventoryManager.instance)
        {
            foreach (InventorySlot slot in InventoryManager.instance.InventorySlotsList)
            {
                if (slot.Item is WeaponInformation weapon)       //if they match, casts to WeaponInformation to add to list of weapons
                {
                    //avoids adding dupes
                    if(!availableWeapons.Contains(weapon))
                        availableWeapons.Add(weapon);
                }
            }
        }
        else
            Debug.Log("No Inventory Manager for weapons");
    }

    //equips the weapon based on the index
    public void EquipWeapon(int index)
    {
        if (index >= 0 && index < availableWeapons.Count)
        {
            //currentWeaponIndex = index;
            gunInfo = availableWeapons[index];

            currentAmmo = gunInfo.maxClipAmmo;
            ammoStored = gunInfo.ammoStored;

            UpdateWeaponModel(gunInfo);
        }
    }

    //update gun model based on the equipped gun
    void UpdateWeaponModel(WeaponInformation _gunInfo)
    {
        gunModelPlaceHolder.GetComponent<MeshFilter>().sharedMesh =
            _gunInfo.ItemModel.GetComponent<MeshFilter>().sharedMesh;

        gunModelPlaceHolder.GetComponent<MeshRenderer>().sharedMaterial =
            _gunInfo.ItemModel.GetComponent<MeshRenderer>().sharedMaterial;
    }

    public void Reload()
    {
        if ((ammoStored > 0 && currentAmmo < gunInfo.maxClipAmmo) || 
            (this.GetComponentInParent<EnemyBase>()))
        {
            if(!isReloading)
                StartCoroutine(ReloadRoutine());
        }
        else if (ammoStored <= 0)
        {
            Debug.Log("Out of Ammo");
        }
    }

    //coroutine for delaying reload
    IEnumerator ReloadRoutine()
    {
        isReloading = true;     //so enemy does not infinite reload
        //sounds/animations
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(gunInfo.reloadRate);

        //refill ammo
        int ammoToRefill = Mathf.Min(gunInfo.maxClipAmmo - currentAmmo, ammoStored);     //makes sure to not use more bullets than stored
        currentAmmo += ammoToRefill;
        ammoStored -= ammoToRefill;

        reloadMessage.SetActive(false);

        isReloading = false;
    }

    public void FireGun()
    {
        //fire only if there is ammo in the gun
        if (currentAmmo > 0 && !isShooting)         //isShooting always false for player
        {
            if (!this.gameObject.CompareTag("Player"))          //only enemy calls coroutine
                StartCoroutine(EnemyShootRate(this.gameObject.GetComponent<EnemyBase>().EnemyShootRate));

            //adjust ammo
            currentAmmo--;

            //raycast to where the gun is aimed at
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hitInfo, gunInfo.shootDistance))
            {
                Debug.Log($"WeaponInAction: Hit {hitInfo.transform.name}");

                //check if it has health to it                                                                                  //APPLY HEALTH/DAMAGE COMPONENT HERE
                HealthSystem targetHealth = hitInfo.transform.GetComponent<HealthSystem>();
                if (targetHealth != null)
                {
                    targetHealth.Damage(gunInfo.shootDamage);
                    Debug.Log($"WeaponInAction: Hit {hitInfo.transform.name} for {gunInfo.shootDamage} damage");
                }

                if (gunInfo.hitEffect != null)
                {
                    Instantiate(gunInfo.hitEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                }
            }
            else
                Debug.Log("WeaponInAction: Missed");

            //muzzle flash method
            //PlayMuzzleFlash();                                                                //UNDER MAINTAINANCE
        }
        else if (this.gameObject.CompareTag("Player"))
        {
            Debug.Log("WeaponInAction: Gun out of ammo");
            reloadMessage.SetActive(true);
        }
    }

    //method to create muzzle flash when shooting the weapon
    void PlayMuzzleFlash()
    {
        if(gunInfo.muzzleFlash != null)
        {
            if (!isFlashing)
            {

                //gunInfo.muzzleFlash.SetActive(true);
                Instantiate(gunInfo.muzzleFlash, gunInfo.muzzleFlashPos.position, gunModelPlaceHolder.transform.rotation);
                StartCoroutine(MuzzleFlashRoutine());
            }
            else


            Debug.Log("WeaponInAction: Muzzle Flash Instantiated");
        }
    }

    IEnumerator EnemyShootRate(int shootRate)
    {
        isShooting = true;

        yield return new WaitForSeconds(shootRate);     //to slow down enemy shoot rate

        isShooting = false;
    }

    IEnumerator MuzzleFlashRoutine()
    {
        isFlashing = true;

        yield return new WaitForSeconds(1f);

        //gunInfo.muzzleFlash.SetActive(false);

        Destroy(gunInfo.muzzleFlash);

        isFlashing = false;
    }
}
