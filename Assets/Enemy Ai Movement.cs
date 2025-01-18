using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< Updated upstream
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
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 720f; // Degrees per second
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Camera mainCamera;

    private Vector3 moveDirection;
>>>>>>> Stashed changes


    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        agent.speed = roamSpeed;
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        if (mainCamera == null)
            mainCamera = Camera.main;
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
        ProcessInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer();
    }

    private void ProcessInput()
    {
        // Capture movement input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
    }

    private void MovePlayer()
    {
        Vector3 velocity = moveDirection * moveSpeed;
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
    }

    private void RotatePlayer()
    {
        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // Draw a gizmo to show detection testing
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 0.5f); // Player indicator
    }
}

