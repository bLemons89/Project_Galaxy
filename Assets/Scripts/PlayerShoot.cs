using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public static event Action shootInput;
    public static event Action weaponReload;

    private void Update()
    {
        if (Input.GetButtonDown("Fire"))
        {
            shootInput?.Invoke();
        }

        if (Input.GetButtonDown("Reload"))
        {           
            weaponReload?.Invoke();
        }
    }


}
