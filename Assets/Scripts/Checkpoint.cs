/*
    Author: Juan Contreras
    Edited By:
    Date Created: 01/16/2025
    Date Updated: 01/17/2025
    Description: Class to use when setting checkpoints in a scene to respawn
                 at when player loses.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    public UnityEvent<Vector3> OnCheckpointSetPos;      //event to pass the checkpoint position to the manager
    //public UnityEvent OnCheckpointActivated;       //to notify other systems a checkpoint has been activated

    [SerializeField] bool isSingleUse = true;   //when active checkpoint can only be used once

    bool isActivated;                           //to keep track if the checkpoint has been activated

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //exit if already activated
            if(isSingleUse && isActivated)
            {
                Debug.Log("Checkpoint has already been activated. Single use.");
                return;
            }

            //activating the checkpoint
            isActivated = true;

            //setting checkpoint through manager
            CheckpointManager.instance.SetCheckpoint(transform.position);


            //to use when notifying other parts of the game (i.e. UI, and Sound)
            //OnCheckpointActivated?.Invoke();

            Debug.Log($"Checkpoint activated at {transform.position}");
        }
    }
}
