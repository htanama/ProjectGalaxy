/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/28/2025
    Date Updated: 02/01/2025
    Description: Triggers the next mission in the queue if the criteria is met
                 (put outside ship)
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective1 : MonoBehaviour
{
    string objectiveID = "1";
    bool playerInRange;

    private void Update()
    {
        if (playerInRange &&
                        !SceneManagerScript.instance.SaveData.IsObjectiveCompleted(objectiveID))        //make sure objective has not been completed before
        {
            ObjectiveManager.instance.CompleteObjective();

            //mark as complete
            SceneManagerScript.instance.SaveData.MarkObjectiveAsCompleted(objectiveID);
            SceneManagerScript.instance.SaveGame();     //save progress

            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
