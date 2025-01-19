using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;


// put the on the emeny object to take damagae by calling HealthSystem
public class TestingTakingDamage : MonoBehaviour
{
    [SerializeField] private HealthSystem trackHealthScript;        

    // Start is called before the first frame update
    void Start()
    {
        WeaponInAction.OnGettingHit += WeaponInAction_OnGettingHit;     
    }

    private void WeaponInAction_OnGettingHit()
    {
        EnemyGotHit();
    }
    private void EnemyGotHit()
    {
        trackHealthScript.Damage(1);
    }
}
