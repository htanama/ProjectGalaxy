using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour //IHealth
{
    [Header("=== PLAYER COMPONENTS ===")]
    [SerializeField] Renderer playerModel;
    [SerializeField] CharacterController playerController;

    //Use when shooting is implemented
    [SerializeField] LayerMask ignoreMask;
    int jumpCount;

    [Header("=== STATS ===")]
    //[SerializeField] private float playerMaxHealth;
    //[SerializeField] private float playerCurrentHealth;

    [Header("=== MOVEMENT ===")]
    [SerializeField][Range(1, 10)] int speed;
    [SerializeField][Range(2, 5)] int sprintMod;
    [SerializeField][Range(1, 5)] int jumpMax;
    [SerializeField][Range(5, 30)] int jumpSpeed;
    [SerializeField][Range(10, 60)] int gravity;

    // Flags //

    //bool isShooting;
    bool isSprinting;
    //bool isPlayingStep;
    //bool isReloading;

    // Cache //
    // Color colorOrig;

    // Vectors //
    Vector3 moveDirection;
    Vector3 horizontalVelocity;


    // Getters and Setters //


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.instance.IsPaused)
        {
            //always checking for these
            Movement();
        }

        Sprint(); //Outside of condition to prevent infinite sprint glitch
    }

    // Player Movement //
    void Movement()
    {
        // resets number of jumps once player is on the ground
        if (playerController.isGrounded)
        {
            jumpCount = 0;

            // falling/ledge
            horizontalVelocity = Vector3.zero;
        }

        // tie movement to camera 
        moveDirection = (transform.right * Input.GetAxis("Horizontal")) +
                        (transform.forward * Input.GetAxis("Vertical"));// normalized to handle diagonal movement
        playerController.Move(moveDirection * speed * Time.deltaTime);

        Jump();

        //physics fix, under object
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
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
            isSprinting = false;
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            jumpCount++;
            horizontalVelocity.y = jumpSpeed;
        }

        //gives jump enemySpeedMult (y) a value
        playerController.Move(horizontalVelocity * Time.deltaTime);
        //start pulling down immediately after the jump
        horizontalVelocity.y -= gravity * Time.deltaTime;




    }


}