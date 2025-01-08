using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponInformation : ScriptableObject
{
    [Header ("Info")]
    public string weaponName;

    [Header("Shooting")]
    public int shootDamage;
    public float shootRate;    
    public int shootDist;
    public int currentAmmo;
    public int maxAmmo;
    public string ammoTypeName;
    public int reloadRate;

    [Header("Audio Info")]
    public ParticleSystem hitEffect;
    public AudioClip[] shootSound;
    public float shootSoundVol;
    public AudioClip reloadSound;
    public float reloadSoundVol;
    public AudioClip emptySound;
    public float emptySoundVol;
    public Sprite weaponSprite;

}
