using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAiMovement2 : MonoBehaviour
{
    [SerializeField] int roamDist;
    [SerializeField] int roamSpeed;
    [SerializeField] int roamTime;
    [SerializeField] NavMeshAgent agent;
    bool isRoaming = false;
    Coroutine cO;
    Vector3 startPos;


    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;

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

}

