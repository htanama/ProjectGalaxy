/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/27/2025
    Date Updated: 01/27/2025
    Description: Class to keep a world counter of shards collected (prevent double count if dropped)
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountShards : MonoBehaviour
{
    bool playerInRange;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Q))
        {
            if (InventoryManager.instance != null)
            {
                //InventoryManager.instance.ShardsCollected++;

                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))                       //ensure it's the player entering the trigger
        {
            playerInRange = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))                       //ensure it's the player leaving the trigger
        {
            playerInRange = false;
        }
    }
}
