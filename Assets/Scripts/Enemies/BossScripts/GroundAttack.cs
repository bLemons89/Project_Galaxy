using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundAttack : MonoBehaviour, IBossAbility
{
    Boss boss;

    [Header("DAMAGE WAVE SETTINGS")]
    [SerializeField] GameObject wavePrefab;
    [SerializeField][Range(5f, 20f)] float maxRadius = 10;
    [SerializeField][Range(5f, 20f)] float innerSafeArea = 8;      //area inside the attack where the player is unaffected
    [SerializeField] int expansionSpeed = 5;
    [SerializeField] int damageAmount = 10;
    [SerializeField][Range(0.5f, 3.0f)] float fadeDuration = 2f;

    Vector3? attackCenter = null;    //to be set once in DamagePlayer()
    float distanceToPlayer;

    public void Initialize(Boss boss)
    {
        this.boss = boss;
    }

    public void Execute()
    {
        Debug.Log("Ground slam attack!");
        //logic for ground slam attack

        boss.StartCoroutine(GroundSlamRoutine());
    }

    IEnumerator GroundSlamRoutine()
    {
        //spawn position for the wave effect
        Vector3 spawnPos = boss.transform.position;
        spawnPos.y = 0f;

        //start wave effect
        GameObject waveInstance = GameObject.Instantiate(wavePrefab, spawnPos, Quaternion.identity);

        //wave material to fade out
        Material waveMaterial = waveInstance.GetComponent<Renderer>()?.material;

        float currentRadius = 0f;
        float alpha = 1f;       //for fade

        while (currentRadius <  maxRadius)
        {
            //increase radius
            currentRadius += expansionSpeed * Time.deltaTime;
            waveInstance.transform.localScale = new Vector3(currentRadius, 1f, currentRadius);

            //damage the player
            DamagePlayer(maxRadius);

            yield return null;
        }
        //reset to null for next attack
        attackCenter = null;

        //fade out effect
        float fadeTimer = 0f;
        while(fadeTimer < fadeDuration)
        {
            fadeTimer += Time.deltaTime;
            alpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeDuration);

            if(waveMaterial != null)
            {
                //update material transparency
                Color color = waveMaterial.color;
                color.a = alpha;
                waveMaterial.color = color;
            }

            yield return null;
        }

        //destroy wave effect
        GameObject.Destroy(waveInstance);
    }

    private void DamagePlayer(float outerRadius)
    {
        //changes at the same rate as the outer radius
        //float innerRadius = outerRadius - innerSafeArea;

        if(attackCenter == null)
            attackCenter = boss.transform.position;

        // if (attackCenter != null)
        //{
        //    float distanceToPlayer = Vector3.Distance(attackCenter.Value, boss.Player.position);
        //}
        if (attackCenter != null)
        {
            //array since this returns an array and could not find one to return a single collider
            Collider[] outerHit = Physics.OverlapSphere(attackCenter.Value, outerRadius);

            foreach (Collider hit in outerHit)
            {
                //check for the player
                if (hit.CompareTag("Player"))
                {
                    //tracking if player jumped (to avoid)
                    //float heightDifference = Mathf.Abs(boss.Player.position.y - boss.Player.position.y);    //Note: Potential bug if player is at a lower level
                    float distanceToCenter = Vector3.Distance(attackCenter.Value, hit.transform.position);

                    //check if player is outside inner safe area
                    if (distanceToCenter >= innerSafeArea/2)
                    {
                        Debug.Log("Player hit by ground attack");
                        //call player take damage with (damageAmount)
                    }
                    else
                        Debug.Log("Player was not hit by ground attack");
                }

                Debug.Log($"if ({attackCenter} >= {innerSafeArea} && ({attackCenter} <= {outerRadius})");
            }
        }
    }
}
