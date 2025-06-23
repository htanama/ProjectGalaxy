using UnityEngine;
using System;
using System.Collections;
using System.IO;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager instance;

    [Header("Player Data from DataScriptableObject")]
    [SerializeField] DataScriptableObject levelInformation;

    [Header("Player Data from PlayerDataScript")]
    public PlayerData data;

    // The location to save the data
    private string saveFolderPath;
    private string saveFilePath;
    private float _playTimer;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;            
    }

    private void Start()
    {
        //_playTimer = SceneManagerScript.instance.gameplayTime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //  This is for testing data loading by pressing "L" Key
    public void LoadDataWithKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadGame();
        }
    }

    //  This is for testing data saving by pressing "P" Key    
    public void SaveDataWithKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveGame();
        }
    }

    // Save player position to a JSON file
    private void SavePlayerData(Vector3 position, float playTimer)
    {
        // Find the CharacterController in the scene
        //CharacterController playerController = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
        //HealthSystem healthEvent = playerController.GetComponent<HealthSystem>();

        data = new PlayerData(position);
        data.playTimer = playTimer;
        //data.CurrentHealth = healthEvent.CurrentHealth; // Current health is not getting the correct data.
        string json = JsonUtility.ToJson(data, true); // Convert to JSON string with pretty print

        File.WriteAllText(saveFilePath, json); // Write JSON string to file    

    }

    public void SaveGame()
    {
        // Define the folder path in the curent directory
        saveFolderPath = Path.Combine(System.Environment.CurrentDirectory, "SaveGame");

        // Create the SaveGame folder if it doesn't exist
        if (!Directory.Exists(saveFolderPath))
        {
            Directory.CreateDirectory(saveFolderPath);
        }
        saveFilePath = Path.Combine(saveFolderPath, "Save.json");
        Debug.Log($"Player location saved to {saveFolderPath}/+{saveFilePath} ");
        Debug.Log("Saving");

        GameObject player = GameObject.FindWithTag("Player");
        SavePlayerData(player.transform.position, _playTimer);
    }


    // Load player position from a JSON file
    private Vector3 LoadPlayerData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath); // Read JSON string from file
            data = JsonUtility.FromJson<PlayerData>(json); // Convert JSON string back to PlayerData

            Vector3 position = new Vector3(data.x, data.y, data.z);
            //Debug.Log("Player location loaded: " + position + "Timer: " + FormatTime(data.playTimer));
            return position;
        }
        else
        {
            Debug.LogWarning("Save file not found. Returning default position.");
            return Vector3.zero; // Default position TODO: Put the correct starting point
        }
    }

    public void LoadGame()
    {
        Vector3 loadPlayerPosition = LoadPlayerData(); // Get saved player position
        
        levelInformation.spawnLocationVec3 = loadPlayerPosition;

        Debug.Log($"Loading Data");

        // Find the CharacterController in the scene
        CharacterController playerController = GameObject.FindWithTag("Player").GetComponent<CharacterController>();

        if (playerController != null)
        {
            // Disable the CharacterController temporarily to update its position
            playerController.enabled = false;
            playerController.transform.position = loadPlayerPosition; // Update position from json file data
            playerController.enabled = true;

            Debug.Log($"Player position updated to: {loadPlayerPosition}");
        }
        else
        {
            Debug.LogError("CharacterController not found in the scene!");
        }
    }
}
