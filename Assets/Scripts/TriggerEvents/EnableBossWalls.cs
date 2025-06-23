/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/27/2025
    Date Updated: 01/27/2025
    Description: Trigger logic to enable the walls around the boss fight area
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableBossWalls : MonoBehaviour
{
    [SerializeField] Boss boss;
    [SerializeField] GameObject[] walls;

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag("Player") && InventoryManager.instance.MissionItemsCollected >= 3)
        {
            boss.enabled = true;
            boss.Start();

            foreach (GameObject wall in walls)
            {
                wall.SetActive(true);
            }
        }*/
    }
}
