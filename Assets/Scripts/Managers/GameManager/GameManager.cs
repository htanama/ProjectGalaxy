using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //singleton
    public static GameManager instance;

    [Header("===== MANAGERS =====")]
    private SceneManagerScript sceneManager;
    private ButtonFunctions buttonFunctions;
    private playerScript _playerScript;
    
    [Header("===== TEMP VARIABLES =====")]
    [SerializeField] GameObject menuActive;
    
    [Header("Cameras")]
    [SerializeField] private Camera gameCamera; 
    [SerializeField] private Camera loadingCamera;
    private Camera currentCamera;

    float timeScaleOrig;
    bool isPaused;
    bool isWebGL;
    public bool IsWebGL
    { get { return isWebGL; } }
    // Pause Events //
    //private GameState currentGameState;
    //public delegate void GameStateChangeHandler(GameState newGameState);
    //public event GameStateChangeHandler OnGameStateChange;
    //public GameState CurrentGameState { get; private set; }

    // Getters and Setters //
    public GameObject MenuActive
    { get => menuActive; set => menuActive = value; }
    public Camera CurrentCamera
    { get => currentCamera; set => currentCamera = value; }
    public Camera GameCamera
    { get => gameCamera; set => gameCamera = value; }
    public Camera LoadingCamera
    { get => loadingCamera; set => loadingCamera = value; }
    public bool IsPaused
    { get => isPaused; set => isPaused = value; }
    public playerScript PlayerScript
    { get => _playerScript; set => _playerScript = value; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        { 
            Destroy(gameObject);
        }
        isWebGL = Application.platform == RuntimePlatform.WebGLPlayer;

        // Set Current GameState
        //currentGameState = GameState.Gameplay;
        //OnGameStateChange?.Invoke(currentGameState);
        timeScaleOrig = Time.timeScale;
        isPaused = false;
        menuActive = null;

        // Instantiate        
        sceneManager = this.GetComponent<SceneManagerScript>();
        buttonFunctions = FindObjectOfType<ButtonFunctions>();
        _playerScript = FindObjectOfType<playerScript>();
    }
    
    // Input //
    void Update()
    {
        // Pause Input
        if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Pause"))
        {
            if (!isPaused)
            {
                StatePause();
                buttonFunctions.OpenPauseMenuBase();
            }
            else if (isPaused)
            {
                buttonFunctions.MenuOpenCheck();

                menuActive = null;

                StateUnpause();
            }           
        }
    }
    public void StatePause()
    {
        //toggle
        isPaused = !isPaused;
        Time.timeScale = 0;
        //cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined; //none can go outside the window/app
    }
    public void StateUnpause()
    {
        //toggle
        isPaused = !isPaused;
        Time.timeScale = timeScaleOrig;
        //cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Pause Buttons //
    public void Resume()
    {
        StateUnpause();
        buttonFunctions.ClosePauseMenuBase();
    }
    /// <summary>
    /// test, make sure restart reloads the player to the last checkpoint (and items)
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        _playerScript.Respawn();
        StateUnpause();        
    }
    /// <summary>
    /// test, make sure save works and updates screen
    /// </summary>
    public void SaveGame()
    {
        //prompt for overwrite, or confirm 
        // call save method
        SceneManagerScript.instance.SaveGame();

        // Stamp
        //string timeStamp = System.DateTime.Now.ToString();
        //buttonFunctions.TimeDateStamp.text = timeStamp;
    }
    /// <summary>
    /// test, make sure load and main menu buttons work
    /// </summary>
    public void LoadGame()
    {
        SceneManagerScript.instance.LoadGame(1);
    }

    public void MainMenu()
    {
        //Navigate to Main Menu Scene
        //remember to disable input?
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        #if UNITY_EDITOR
                // Stop play mode in the editor
                UnityEditor.EditorApplication.isPlaying = false;
        #endif

        #if UNITY_WEBGL
            // reload the page on quit
            Application.OpenURL(Application.absoluteURL); // This reloads the page, effectively restarting the game
        #endif

        #if !UNITY_EDITOR && !UNITY_WEBGL
            Application.Quit();
        #endif
    }

}
