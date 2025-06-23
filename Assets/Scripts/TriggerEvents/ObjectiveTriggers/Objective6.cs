/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/28/2025
    Date Updated: 02/01/2025
    Description: Triggers the next mission in the queue if the criteria is met
                 (place on top of trigger 5)
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective6 : MonoBehaviour
{
    string objectiveID = "6";
    bool playerInRange;

    private void Update()
    {
        if (!SceneManagerScript.instance.SaveData.IsObjectiveCompleted(objectiveID))
        {
            if (playerInRange && Input.GetKeyDown(KeyCode.Q) &&
                SceneManagerScript.instance.SaveData.energyCellsCollected >= 2)
            {
                //mark as complete
                SceneManagerScript.instance.SaveData.MarkObjectiveAsCompleted(objectiveID);
                SceneManagerScript.instance.SaveGame();     //save progress

                //iterate to next objective
                InventoryManager.instance.CollectEnergyCell();
                ObjectiveManager.instance.CompleteObjective();
                Destroy(gameObject);
            }
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
