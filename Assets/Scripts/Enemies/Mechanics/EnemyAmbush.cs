/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/25/2025
    Date Updated: 01/25/2025
    Description: Script to put on a trigger to have all enemies go to the player's location
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAmbush : MonoBehaviour
{
    public Transform player;
    private void OnTriggerEnter(Collider other)
    {
        //check if object has a NavMeshAgent
        NavMeshAgent agent = other.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            //set the agent's destination to the player's location
            agent.SetDestination(player.position);
            //Debug.Log($"{other.gameObject.name} is now targeting the player!");
        }
    }
}
