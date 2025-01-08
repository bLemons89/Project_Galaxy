using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponInformation : ScriptableObject
{
    public GameObject gameObject;

    public int shootDamage;
    public float shootRate;
    public int reloadRate;
    public int shootDist;
    public int currentAmmo;
    public int maxAmmo;
    public string ammoTypeName;

    public ParticleSystem hitEffect;
    public AudioClip[] shootSound;
    public float shootSoundVol;
    public AudioClip reloadSound;
    public float reloadSoundVol;
    public AudioClip emptySound;
    public float emptySoundVol;
    public Sprite weaponSprite;

}
