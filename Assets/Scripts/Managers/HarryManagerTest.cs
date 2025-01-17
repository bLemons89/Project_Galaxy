using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HarryManagerTest : MonoBehaviour
{
    [SerializeField] public TMP_Text currentAmmoText;
    [SerializeField] GameObject WeaponOnPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (WeaponOnPlayer != null && WeaponOnPlayer.GetComponent<WeaponInformation>() != null)
        //{
        //    //currentAmmoText = WeaponOnPlayer.GetComponent<WeaponInformation>().currentAmmo.ToString();
        //}
    }
}
