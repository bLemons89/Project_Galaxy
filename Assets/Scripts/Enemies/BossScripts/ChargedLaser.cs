using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedLaser : IBossAbility
{
    Boss boss;

    public void Initialize(Boss boss )
    {
        this.boss = boss;
    }

    public void Execute()
    {
        Debug.Log("Charging laser ability");
        //charged laser logic
    }
}
