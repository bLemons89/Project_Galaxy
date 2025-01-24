using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{
    [Header("===== PLAYER COMPONENTS =====")]
    [SerializeField] Renderer playerModel;
    [SerializeField] CharacterController playerController;

    [SerializeField] LayerMask ignoreMask;
    int jumpCount;

    [Header("===== STATS =====")]

    [Header("===== MOVEMENT =====")]
    [SerializeField][Range(1, 10)] int speed;
    [SerializeField][Range(2,  5)] int sprintMod;
    [SerializeField][Range(1,  5)] int jumpMax;
    [SerializeField][Range(5, 30)] int jumpSpeed;
    [SerializeField][Range(10,60)] int gravity;

    // Flags //
    bool isSprinting;
    //bool isShooting;
    bool isPlayingStep;
    //bool isReloading
    bool isStunned;

    // Cache //

    // Vectors //
    Vector3 moveDirection;
    Vector3 horizontalVelocity;
    //vector to store checkpoint


    // Getters and Setters //
    public int Speed => speed;  //stun enemy uses this
    public int SprintMod => sprintMod; //stun enemy

    
    void Start()
    {
     
    }

    void Update()
    {
        //no movement input sent out while stunned
        if (isStunned)
            return;

        if(!GameManager.instance.IsPaused)
        {
            //always checking for
            Movement();
        }
        
        //Ouside condition to prevent infinite glitch
        Sprint();
    }

    // Player Movement //
    void Movement()
    {

        //resets jumps once player is on the ground
        if (playerController.isGrounded)
        {
            if (moveDirection.magnitude > 0.3f && !isPlayingStep)
            {
                StartCoroutine(PlayStep());
                //AudioManager2.PlaySound(AudioManager2.Sound.PlayerMove);
            }

            jumpCount = 0;

            //falling/ledge
            horizontalVelocity = Vector3.zero;
        }

        //tie movement to camera, normalized to handle diagonal movement
        moveDirection = (transform.right * Input.GetAxis("Horizontal")) +
                        (transform.forward * Input.GetAxis("Vertical"));

        playerController.Move(moveDirection * speed * Time.deltaTime);

        Jump();

        //physics fix for under object
        if ((playerController.collisionFlags & CollisionFlags.Above) != 0)
        {
            horizontalVelocity.y = Vector3.zero.y;
        }
    }

    void Sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
            isSprinting = true;
        }
        else if(Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
            isSprinting = false;
        }
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            jumpCount++;
            horizontalVelocity.y = jumpSpeed;
            // Audio Play from manager 2
            //AudioManager2.PlaySound(AudioManager2.Sound.PlayerJump);
            AudioManager.instance.PlaySFX("Jump", AudioManager.instance.playerSFX);
        }

        playerController.Move(horizontalVelocity * Time.deltaTime);
        horizontalVelocity.y -= gravity * Time.deltaTime;
    }

    public void Respawn()                   //called using Unity event
    {
        if (CheckpointManager.instance)
        {
            Vector3 respawnPosition = CheckpointManager.instance.LastCheckpointPosition;

            Debug.Log($"Last Checkpoint position stored for respawn at {respawnPosition}");

            //disable controller to move player
            playerController.enabled = false;
            transform.position = respawnPosition;
            playerController.enabled = true;

            //resetting speed to prevent glitches
            horizontalVelocity = Vector3.zero;

            Debug.Log($"Player respawned at {respawnPosition}");
        }
        else
        {
            Debug.Log("No CheckpointManager, unable to respawn");
        }
    }

    public void Stun(float duration)        //called from stun enemy
    {
        //add stun effect logic
        Debug.Log($"Player stunned for {duration} seconds");

        if(!isStunned)
            StartCoroutine(StunRoutine(duration));
    }

    IEnumerator StunRoutine(float duration)
    {
        //disable movement/actions
        isStunned = true;

        //stun duration
        yield return new WaitForSeconds(duration);

        //enable movement/actions
        isStunned = false;
    }
    IEnumerator PlayStep()
    {
        isPlayingStep = true;

        AudioManager.instance.PlaySFX("Steps", AudioManager.instance.playerSFX);
        //playerAudio.PlayOneShot(audStep[Random.Range(0, audStep.Length)], audStepVol);

        if (!isSprinting)
        {
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
        }

        isPlayingStep = false;
    }

}
