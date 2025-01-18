using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedLaser : IBossAbility
{
    [SerializeField] [Range(0.5f, 5f)] float chargeTime = 2.0f;             //time to charge the laser
    [SerializeField] [Range(0.3f, 3f)] float lockOnShootDelay = 1.0f;       //delay to shoot once boss locks on

    Boss boss;
    Vector3 lockedShootPos;

    public void Initialize(Boss boss )
    {
        this.boss = boss;
    }

    public void Execute()
    {
        Debug.Log("Charging laser ability");

        if (boss.Player != null)
        {
            boss.StartCoroutine(ChargeLaserRoutine());
        }
        else
            Debug.Log("Boss(ChargedLaser): No player reference found!");
    }

    IEnumerator ChargeLaserRoutine()
    {
        //charge and aim laser
        float timer = 0f;
        while(timer < chargeTime)
        {
            timer += Time.deltaTime;

            //boss aims at player while charging
            AimAtPlayer();

            yield return null;
        }

        //after laser is charged, lock position to shoot
        lockedShootPos = boss.Player.position;

        Debug.Log($"Laser locked at {lockedShootPos}");

        yield return new WaitForSeconds(lockOnShootDelay);

        //shoot
        ShootLaser(lockedShootPos);
    }

    void AimAtPlayer()
    {
        Transform player = boss.Player;
        if (player != null)
        {
            //turn to look at the player
            Vector3 directionToPlayer = player.position - boss.transform.position;
            directionToPlayer.y = 0;
            boss.transform.rotation = Quaternion.LookRotation(directionToPlayer);
        }
    }

    void ShootLaser(Vector3 targetPosition)
    {
        Debug.Log($"Boss: Firing laser at {targetPosition}");
        //shoot laser logic


    }
}
