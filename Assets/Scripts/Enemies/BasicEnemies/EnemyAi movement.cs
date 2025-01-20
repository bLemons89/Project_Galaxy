using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAiMovement : MonoBehaviour
{
    [SerializeField] int roamDist;
    [SerializeField] int roamSpeed;
    [SerializeField] int roamTime;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] float shootRate;
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    [SerializeField] Animator animator;
    bool isRoaming = false;
    bool isShooting;
    Coroutine cO;
    Vector3 startPos;
    Color colorOrig;


    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        colorOrig = model.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRoaming)
        {
            cO = StartCoroutine(roam());
        }
    }

    IEnumerator roam()
    {
        isRoaming = true;
        yield return new WaitForSeconds(roamTime);
        Vector3 randPos = Random.insideUnitSphere * roamDist;
        randPos += startPos;
        NavMeshHit hit;
        NavMesh.SamplePosition(randPos, out hit, roamDist, 1);
        agent.SetDestination(hit.position);
        isRoaming = false;

    }

    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }

    // Enemy Shoot //
    IEnumerator shoot()
    {
        // turn on
        isShooting = true;

        // animation
        animator.SetTrigger("Shoot");

        // create bullet
        Instantiate(bullet, shootPos.position, transform.rotation);

        // enemySpeedMult
        yield return new WaitForSeconds(shootRate);

        // turn off
        isShooting = false;
    }

}

