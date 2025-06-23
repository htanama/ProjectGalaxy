/*
    Author: Juan Contreras
    Edited by:
    Date Created: 02/02/2025
    Date Updated: 02/02/2025
    Description: Triggers the next mission in the queue if the criteria is met
                 (Objective 5 and 6)
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinedObjectives : MonoBehaviour
{
    private bool playerInRange;

    private void Update()
    {
        if (!SceneManagerScript.instance.SaveData.IsObjectiveCompleted("6") && playerInRange && Input.GetKeyDown(KeyCode.Q))
        {
            //objective 6
            if (SceneManagerScript.instance.SaveData.energyCellsCollected >= 2)
            {
                //mark as complete
                SceneManagerScript.instance.SaveData.MarkObjectiveAsCompleted("6");
                SceneManagerScript.instance.SaveGame();     //save progress

                //iterate to next objective
                InventoryManager.instance.CollectEnergyCell();
                ObjectiveManager.instance.CompleteObjective();
                Destroy(gameObject);
            }
            else if (!SceneManagerScript.instance.SaveData.IsObjectiveCompleted("5"))       //objective 5
            {
                //otherwise, if less than 2 energy cells have been collected,
                //execute the first behavior.
                SceneManagerScript.instance.SaveData.MarkObjectiveAsCompleted("5");
                SceneManagerScript.instance.SaveGame();     // Save progress

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