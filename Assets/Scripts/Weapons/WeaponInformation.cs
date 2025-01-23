using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Weapon Information")]

public class WeaponInformation : ItemBase
{
    [Header("Info")]
    public string weaponName;

    [Header("Shooting")]
    public int shootDamage;
    public float shootRate;
    public int shootDistance;
    public int currentAmmo;
    public int maxClipAmmo;
    public int ammoStored;
    public string ammoTypeName;
    public int reloadRate;
    public ParticleSystem hitEffect;
    public GameObject muzzleFlash;

    [Header("Area Damage")]
    public float areaOfEffectRadius;
    public int splashDamage;

    [Header("Sprite Info for UI")]
    public Sprite weaponSprite; // sprite to show UI weapon changes, if needed.

    public override void IntendedUse()
    {
        throw new System.NotImplementedException();
    }

    // Anything below are from the class PP2, I am not sure if you would like to use these:
    [Header("Audio Info")]        
    public AudioClip[] shootSound;
    public float shootSoundVol;
    public AudioClip reloadSound;
    public float reloadSoundVol;
    public AudioClip emptySound;
    public float emptySoundVol;

}
