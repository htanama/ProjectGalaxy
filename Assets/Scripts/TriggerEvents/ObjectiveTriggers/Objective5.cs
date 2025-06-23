/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/28/2025
    Date Updated: 02/01/2025
    Description: Triggers the next mission in the queue if the criteria is met
                 (place on top of cell with other trigger(6))
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective5 : MonoBehaviour
{
    string objectiveID = "5";
    bool playerInRange;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Q))
        {
            if (!SceneManagerScript.instance.SaveData.IsObjectiveCompleted(objectiveID))
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
