/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/26/2025
    Date Updated: 02/01/2025
    Description: Trigger logic to enable a collider to block the entrance to the ship during boss fight
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockShipEntrance : MonoBehaviour
{
    [SerializeField] GameObject entranceCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (!SceneManagerScript.instance.SaveData.IsObjectiveCompleted("8"))        //trigger if boss has not been defeated (objective 8)
        {
            if (other.CompareTag("Player") && SceneManagerScript.instance.SaveData.energyCellsCollected >= 3)      //maybe link to another number, works for now
            {
                entranceCollider.SetActive(true);
            }
        }
    }
}
