using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeaponInAction : MonoBehaviour
{
    private static WeaponInAction Instance;

    [Header("PUT YOUR WEAPON PREFABS MODELS SAMPLES HERE")]
    [SerializeField] GameObject assaultRifleModel;
    [SerializeField] GameObject shotgunModel;
    [SerializeField] GameObject energyRifleModel;

    [Header("PUT THE WEAPON SCRIPTABLE OBJECT INFO HERE")]
    [SerializeField] WeaponInformation assaultRifleScriptableObject;
    [SerializeField] WeaponInformation shotgunScriptableObject;
    [SerializeField] WeaponInformation energyRifleScriptableObject;

    [Header("Gun Model Place Holder")]
    [SerializeField] GameObject gunModelPlaceHolder;

    [Header("Reload UI Message")]
    [SerializeField] GameObject relaodMessage;
    [SerializeField] TMP_Text reloadText;

    private RangedEnemy localRangeEnemy;

    public static event Action OnBulletProjectile;
    public static event Action OnGettingHit;

    private WeaponInformation gunInfo;
    // Is the object getting shot? 
    private bool isShot = false;

    private string weaponKeyMap = string.Empty;
    // checks weapon on the inventory
    private bool hasAssaultRifle = false;
    private bool hasEnergyRifle = false;
    private bool hasShotgunRifle = false;
    private bool isSwitchWeapon = false;
    private int inventoryIndex = 0;
    private int numberOfWeapon = 0;
    private int numberOfAmmo = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instance.
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Make this GameObject persistent.
    }

    private void Start()
    {
        PlayerShoot.OnShootInput += PlayerShoot_shootInput;
        PlayerShoot.OnWeaponReload += Reload;
        TakingAmmo.OnTakingAmmo += TakingAmmo_OnTakingAmmo;
    }

    private void TakingAmmo_OnTakingAmmo()
    {
        numberOfAmmo++;        
    }

    // Example from Unity: Draws a 10 meter long green line from the position for 1 frame.
    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(transform.position, forward, Color.green);

        if (InventoryManager.instance.InventorySlotsList.Count > 0)
        {
            CheckWeaponInventory();
        }
        
        SwitchWeapon();

        // press P to Restart Scene
        if (Input.GetButtonDown("Restart Scene"))
        {
            SceneManagerScript.instance.ResetScene();
        }
       
    }

    private void PlayerShoot_shootInput()
    {
        if (gunInfo != null && gunInfo.currentAmmo > 0)
        {
            // check if the raycast hit object
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, gunInfo.shootDistance))
            {
                // Bullet need to start moving here
                // OnBulletProjectile?.Invoke();

                Debug.Log(hitInfo.transform.name + $" Got Hit");

                // we need to get enemy damage here
                // OnGettingHit?.Invoke();
                

                isShot = true; // got shot

                gunInfo.currentAmmo--;

                if (gunInfo.currentAmmo < 2)
                {
                    relaodMessage.SetActive(true);
                }                             
            }
            else
            {
                isShot = false; // did not get shot
            }

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
        if (numberOfAmmo > 0)
        {
            gunInfo.currentAmmo = gunInfo.maxAmmo;
            --numberOfAmmo;
            relaodMessage.SetActive(false);
        }
    }

    public int GetShootDamage()
    {
        return gunInfo.shootDamage;
    }

    public bool IsShot()
    {
        return isShot;
    }

    private void CheckWeaponInventory()
    {       
        InventorySlot myInventorySlot;


        while (inventoryIndex < InventoryManager.instance.InventorySlotsList.Count && numberOfWeapon < 3)
        {
            myInventorySlot = InventoryManager.instance.InventorySlotsList[inventoryIndex];

            if (myInventorySlot.Item.ItemName == "AR")
            {                
                hasAssaultRifle = true;
                CurrentWeapon(assaultRifleModel, assaultRifleScriptableObject);
                numberOfWeapon++;
            }                  
            else if (myInventorySlot.Item.ItemName == "SG")
            {
                hasShotgunRifle = true;
                CurrentWeapon(shotgunModel, shotgunScriptableObject);
                numberOfWeapon++;
            }
            else if (myInventorySlot.Item.ItemName == "ER")
            {
                hasEnergyRifle = true;
                CurrentWeapon(energyRifleModel, energyRifleScriptableObject);
                numberOfWeapon++;
            }

            inventoryIndex++;
        }
        
    }

    private void SwitchWeapon()
    {
        if (Input.GetButtonDown("Number One") && hasAssaultRifle)
        {
            isSwitchWeapon = true;
            CurrentWeapon(assaultRifleModel, assaultRifleScriptableObject);         
        }
        else if (Input.GetButtonDown("Number Two") && hasShotgunRifle)
        {
            isSwitchWeapon = true;
            CurrentWeapon(shotgunModel, shotgunScriptableObject);
        }
        else if (Input.GetButtonDown("Number Three") && hasEnergyRifle)
        {
            isSwitchWeapon = true;
            CurrentWeapon(energyRifleModel, energyRifleScriptableObject);
        }
        else
        {
            isSwitchWeapon = false;
        }
    }

    private void CurrentWeapon(GameObject weaponModel, WeaponInformation weaponInfo)
    {
        gunModelPlaceHolder.GetComponent<MeshFilter>().sharedMesh = weaponModel.GetComponent<MeshFilter>().sharedMesh;
        gunModelPlaceHolder.GetComponent<MeshRenderer>().sharedMaterial = weaponModel.GetComponent<MeshRenderer>().sharedMaterial;
        gunInfo = weaponInfo;
    }

    public void ResetWeaponData()
    {
        assaultRifleScriptableObject.currentAmmo = assaultRifleScriptableObject.maxAmmo;
        shotgunScriptableObject.currentAmmo = shotgunScriptableObject.maxAmmo;
        energyRifleScriptableObject.currentAmmo = energyRifleScriptableObject.maxAmmo;
    }

}
