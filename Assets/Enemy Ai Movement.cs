using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAiMovement : MonoBehaviour
{
    [SerializeField] private int roamDist;
    [SerializeField] private int roamSpeed;
    [SerializeField] private int roamTime;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float detectionRadius = 15f;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float attackCooldown = 1.5f;

    private bool isRoaming = false;
    private bool canAttack = true;
    private Coroutine roamCoroutine;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        agent.speed = roamSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            StopRoaming();
            ChasePlayer();
        }
        else if (!isRoaming)
        {
            roamCoroutine = StartCoroutine(Roam());
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        FacePlayer();

        if (agent.remainingDistance <= agent.stoppingDistance && canAttack)
        {
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        canAttack = false;

        // Instantiate and shoot projectile
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = (player.position - shootPoint.position).normalized * 10f;

        // Start cooldown coroutine
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private IEnumerator Roam()
    {
        isRoaming = true;

        while (true)
        {
            yield return new WaitForSeconds(roamTime);
            Vector3 randomPos = Random.insideUnitSphere * roamDist + startPos;

            if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, roamDist, 1))
            {
                agent.SetDestination(hit.position);
            }
        }
    }

    private void StopRoaming()
    {
        if (roamCoroutine != null)
        {
            StopCoroutine(roamCoroutine);
            roamCoroutine = null;
            isRoaming = false;
        }
    }

    private void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}

