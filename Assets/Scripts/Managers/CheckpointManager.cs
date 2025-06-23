/*
    Author: Juan Contreras
    Edited By:
    Date Created: 01/16/2025
    Date Updated: 01/31/2025
    Description: Class to manage checkpoints the player sets off in game
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager instance;

    //Vector3 lastCheckpointPosition;

    //public Vector3 LastCheckpointPosition => lastCheckpointPosition;

    // Awake is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    //call this from the checkpoint trigger (position = transform.position)
    public void SetCheckpoint(Vector3 position)
    {
        if (SceneManagerScript.instance != null)
        {
            //store checkpoint in SaveData (for JSON file)
            SceneManagerScript.instance.SaveData.SaveCheckpoint(SceneManager.GetActiveScene().name, position);

            //auto save game (writes to the JSON file)
            SceneManagerScript.instance.SaveGame();

            //Debug.Log($"Checkpoint set at: {SceneManagerScript.instance.SaveData.lastCheckpointPosition}");
        }
    }
}
