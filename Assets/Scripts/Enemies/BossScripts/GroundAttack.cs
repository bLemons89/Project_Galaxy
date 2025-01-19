using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundAttack : IBossAbility
{
    Boss boss;

    public void Initialize(Boss boss)
    {
        this.boss = boss;
    }

    public void Execute()
    {
        Debug.Log("Ground slam attack!");
        //logic for ground slam attack
    }
}
