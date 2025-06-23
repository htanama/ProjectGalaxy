/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/28/2025
    Date Updated: 02/01/2025
    Description: Triggers the next mission in the queue if the criteria is met
 */
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Objective0 : MonoBehaviour
{
    [SerializeField] GameObject exitCollider;

    string objectiveID = "0";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") &&
        InventoryManager.instance.InventorySlotsList.Find(slot => slot.Item.ItemType_ == ItemBase.ItemType.Weapon) != null)
        {
            if (!SceneManagerScript.instance.SaveData.IsObjectiveCompleted(objectiveID))
            {

                ObjectiveManager.instance.CompleteObjective();

                //mark as complete
                SceneManagerScript.instance.SaveData.MarkObjectiveAsCompleted(objectiveID);

                //mark as destroyed for saving
                SceneManagerScript.instance.MarkObjectAsDestroyed(this.GetComponent<UniqueID>().ID);
                SceneManagerScript.instance.MarkObjectAsDestroyed(exitCollider.GetComponent<UniqueID>().ID);
                SceneManagerScript.instance.SaveGame();     //save progress

                Destroy(exitCollider);

                Destroy(gameObject);        //remove trigger once complete
            }
        }
    }
}
