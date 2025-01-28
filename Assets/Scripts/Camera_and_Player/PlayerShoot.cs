using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

// Attach this script on the Player Prefab to use

public class PlayerShoot : MonoBehaviour
{
    public static event Action OnShootInput;
    public static event Action OnWeaponReload;        

    private void Update()
    {
        if (Input.GetButtonDown("Fire"))
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.Weapons, "AR_Sound");
            OnShootInput?.Invoke();
        }

        if (Input.GetButtonDown("Reload"))
        {           
            OnWeaponReload?.Invoke();
        }
    }    


}
