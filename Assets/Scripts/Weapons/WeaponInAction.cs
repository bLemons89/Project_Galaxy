/*
    Author: Harry Tanama
    Edited by: Juan Contreras
    Date Created: 01/18/2025
    Date Updated: 01/22/2025
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
    //public static event Action OnBulletProjectile;
    //public static event Action OnGettingHit;

    [Header("WEAPON INFO")]
    [SerializeField] List<WeaponInformation> availableWeapons = new List<WeaponInformation>();   //player and enemy can use
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
    //===========GETTERS===========
    public int CurrentAmmo => currentAmmo;

    public GameObject GunModelPlaceHolder => gunModelPlaceHolder;
    public WeaponInformation GunInfo { get; set; }

    void Start()
    {
        //subscribed events
        //PlayerShoot.OnWeaponReload += Reload;
        //TakingAmmo.OnTakingAmmo += TakingAmmo_OnTakingAmmo;


        /*if(this.gameObject.layer == 6)      //enemy is on layer 6
        {
            EquipWeapon(0);

            currentAmmo = gunInfo.maxClipAmmo;
            ammoStored = gunInfo.ammoStored;
        }*/

    }

    // Example from Unity: Draws a 10 meter long green line from the position for 1 frame.
    void Update()
    {
        //Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        //Debug.DrawRay(transform.position, forward, Color.green);

        /*
        if (InventoryManager.instance.InventorySlotsList.Count > 0)
        {
            CheckWeaponInventory();
        }
        */

        //OnSwitchWeapon();

    }

    public void OnSwitchWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && availableWeapons.Count > 0)        //press 1 for primary
        {
            EquipWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && availableWeapons.Count > 0)
        {
            EquipWeapon(1);
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

        //gunModelPlaceHolder.GetComponent<MeshFilter>().sharedMesh.

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
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, gunInfo.shootDistance))
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
            PlayMuzzleFlash();
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
            Instantiate(gunInfo.muzzleFlash, gunModelPlaceHolder.transform.position, gunModelPlaceHolder.transform.rotation);

            Debug.Log("WeaponInAction: Muzzle Flash Instantiated");
        }
    }

    IEnumerator EnemyShootRate(int shootRate)
    {
        isShooting = true;

        yield return new WaitForSeconds(shootRate);     //to slow down enemy shoot rate

        isShooting = false;
    }
    /*
        private void TakingAmmo_OnTakingAmmo()
        {
            ammoStored++;        
        }

        public void UpdateAmmo()
        {
            if(gunInfo != null)
                currentAmmo = gunInfo.currentAmmo;
        }

        private void PlayerShoot_shootInput()
        {
            //couldn't get this to go off with just the weapon
            //if (PlayerShoot.OnShootInput() != null && !isShot)
            AudioManager2.PlaySound(AudioManager2.Sound.Weapon1Shoot);


            if (currentAmmo > 0)
            {
                if(reloadMessage.activeSelf)
                    reloadMessage.SetActive(false);

                // check if the raycast hit object
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, gunInfo.shootDistance))
                {
                    // Bullet need to start moving here
                    // OnBulletProjectile?.Invoke();

                    Debug.Log(hitInfo.transform.name + $" Got Hit");

                    // we need to get enemy damage here
                    // OnGettingHit?.Invoke();
                    if (gunInfo != null)
                    {
                        HealthSystem enemyHealthSystem = hitInfo.transform.GetComponent<HealthSystem>();
                        enemyHealthSystem.Damage(1);
                        AudioManager2.PlaySound(AudioManager2.Sound.EnemyDamage);
                    }

                    isShot = true; // got shot
                    // Hit Effect for the weaopons on enemies
                    WeaponInformation.Instantiate(gunInfo.hitEffect, hitInfo.point, Quaternion.identity);


                    //exits if statement when used
                    /*if (currentAmmo < 2)
                    {

                    }
                }
                else
                {
                    isShot = false; // did not get shot
                }

                currentAmmo--;

                Debug.Log($"Current Ammo: {currentAmmo}");
            }
            else if (gunInfo)
            {
                if(!reloadMessage.activeSelf)
                    reloadMessage.SetActive(true);

            }
        }*/
}
