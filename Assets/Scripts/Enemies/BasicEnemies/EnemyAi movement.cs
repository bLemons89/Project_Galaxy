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
    [SerializeField] Transform headPos;
    [SerializeField] int facePlayerSpeed;
    int stoppingDistanceOrg;
   
    bool isRoaming = false;
    bool isShooting;
    //bool playerInRange;
    Coroutine cO;
    Vector3 startPos;
    Vector3 playerDir;
    Color colorOrig;
    float angleToPlayer;
    playerScript PlayerScript;

    // Start is called before the first frame update
    void Start()
    {
        PlayerScript = FindObjectOfType<playerScript>();
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

    
    IEnumerator shoot()
    {
       
        isShooting = true;

       
        animator.SetTrigger("Shoot");

        
        Instantiate(bullet, shootPos.position, transform.rotation);

       
        yield return new WaitForSeconds(shootRate);

        //call shoot sound
        isShooting = false;
    }

    
    bool canSeePlayer()
    {
        
        playerDir = PlayerScript.Player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);

        
        RaycastHit hit;
        
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            
            agent.stoppingDistance = stoppingDistanceOrg;

            
            if (hit.collider.CompareTag("Player"))
            {
                
                agent.SetDestination(PlayerScript.Player.transform.position);

                
                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    faceTarget();
                }

                
                if (!isShooting)
                {
                    StartCoroutine(shoot());
                }
                
                return true;
            }
        }
        
        agent.stoppingDistance = 0;

        
        return false;
    }


   
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            //playerInRange = true;
        }

    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //playerInRange = false;
            agent.stoppingDistance = 0; 
        }

    }

    
    void faceTarget()
    {
        
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));

       
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * facePlayerSpeed);
    }

    
    

}

