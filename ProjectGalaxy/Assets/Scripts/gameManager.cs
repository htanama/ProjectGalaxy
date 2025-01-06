using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    //singleton
    public static gameManager instance;

    [Header("=== OVERLAYS ===")]
    [SerializeField] public GameObject backgroundScreen;

    [Header("=== PLAYER ===")]
    private GameObject player;
    private playerScript playerScript;

    [Header("=== TEMP VARIABLES ===")]
    [SerializeField] GameObject menuActive;

    // Flags //
    private bool isPaused;

    // Cache //
    /* Save original time scale for pause/resume */
    float timeScaleOrig;     

    // Getters and Setters //

    /* Placeholder: player and player script getters and setters or read only */

    public bool IsPaused
    {
        get => isPaused;
        set => isPaused = value;
    }

    // Start //
    void Awake()
    {
        instance = this;
        
        // Set Original Values //
        timeScaleOrig = Time.timeScale;

        // Find and Set Player Reference //
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerScript>();

    }

    // Update //
    void Update()
    {
        if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Pause"))
        {
            /* 
                Press esc or tab to pause, 
                background Screen will appear 
            */
            if (menuActive == null)
            {
                StatePause();
                menuActive = backgroundScreen;
                menuActive.SetActive(true);
            }
            else if (menuActive == backgroundScreen)
            {
                StateUnPause();
            }
        }
    }

    // Game States //
    public void StatePause()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void StateUnPause()
    {
        isPaused = !isPaused;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }

    /* Win, Lose, Menus, Update UI */



}
