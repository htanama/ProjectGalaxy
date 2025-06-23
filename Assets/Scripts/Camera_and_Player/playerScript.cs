using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playerScript : MonoBehaviour
{
    [Header("===== PLAYER COMPONENTS =====")]
    private GameObject player;
    private playerScript _playerScript;
    [SerializeField] GameObject playerDamageScreen;
    [SerializeField] Renderer playerModel;
    [SerializeField] CharacterController playerController;
    [SerializeField] Animator animator;

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
    bool isPlayingStep;
    //bool isShooting;
    //bool isReloading
    bool isStunned;

    // Vectors //
    Vector3 moveDirection;
    Vector3 horizontalVelocity;
    //vector to store checkpoint
    cameraController playerCamera;

    // Getters and Setters //
    public int Speed => speed;  //stun enemy uses this
    public int SprintMod => sprintMod; //stun enemy

    public GameObject Player => player;
    public GameObject PlayerDamageScreen
    { get => playerDamageScreen; set => playerDamageScreen = value; }
    public playerScript PlayerScript
    { get => _playerScript; set => _playerScript = value; }

    float origTime;

    void Start()
    {
        // Subscribe to the State Changes
        origTime = Time.timeScale;

        // find and set player reference
        player = GameObject.FindWithTag("Player");
        _playerScript = player.GetComponent<playerScript>();
        playerCamera = GameObject.FindWithTag("MainCamera").GetComponent<cameraController>();

        playerDamageScreen = GameObject.Find("PlayerDmgScreen");
        if(playerDamageScreen != null)
            playerDamageScreen.GetComponent<Image>().enabled = false;
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
            AudioManager.instance.PlaySFX(AudioManager.instance.PlayerJump[Random.Range(0,
                AudioManager.instance.PlayerJump.Length)]);
        }
        playerController.Move(horizontalVelocity * Time.deltaTime);
        horizontalVelocity.y -= gravity * Time.deltaTime;
    }

    public void Respawn()                   //called using Unity event
    {
        if (SceneManagerScript.instance != null)
        {
            Time.timeScale = origTime;

            Vector3 respawnPosition;

            //look for a checkpoint in the scene
            if(SceneManagerScript.instance.SaveData.lastCheckpointPositions.Exists(cp => cp.sceneName == SceneManager.GetActiveScene().name))
            {
                respawnPosition = SceneManagerScript.instance.SaveData.GetCheckpointPosition(SceneManager.GetActiveScene().name);
            }
            else
            {
                respawnPosition = transform.position;
            }

            //Debug.Log($"Last Checkpoint position stored for respawn at {respawnPosition}");

            //disable controller to move player
            playerController.enabled = false;
            transform.position = respawnPosition;
            playerController.enabled = true;

            //resetting speed to prevent glitches
            horizontalVelocity = Vector3.zero;

            //Debug.Log($"Player respawned at {respawnPosition}");
        }
        else
        {
            //Debug.Log("No SceneManagerScript, unable to respawn");
        }
    }

    public void Stun(float duration, float stunSensitivity)        //called from stun enemy
    {
        //add stun effect logic
        //Debug.Log($"Player stunned for {duration} seconds");

        if(!isStunned)
            StartCoroutine(StunRoutine(duration, stunSensitivity));
    }

    IEnumerator StunRoutine(float duration, float stunSensitivity)
    {
        //disable movement/actions
        isStunned = true;

        //adjust camera sensitivity
        playerCamera.Sensitivity = stunSensitivity;

        //stun duration
        yield return new WaitForSeconds(duration);

        //restore camera sensitivity
        playerCamera.Sensitivity = playerCamera.OrigSensitivity;

        //enable movement/actions
        isStunned = false;
    }
    IEnumerator PlayStep()
    {
        isPlayingStep = true;
        // Player step audio here
        AudioManager.instance.PlaySFX(AudioManager.instance.PlayerWalk[Random.Range(0,
            AudioManager.instance.PlayerWalk.Length)]);

        if (!isSprinting)
        {
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            yield return new WaitForSeconds(0.2f);
        }

        isPlayingStep = false;
    }
    //private void OnGameStateChange(GameState newGameState)
    //{
    //    if(newGameState == GameState.Pause)
    //    {
    //        this.enabled = false;
    //    }
    //    else if(newGameState == GameState.Gameplay)
    //    {
    //        this.enabled = true;
    //    }
    //}
    //private void OnDestroy()
    //{
    //    // Unsubscribe
    //    GameManager.instance.OnGameStateChange -= OnGameStateChange;
    //}
}
