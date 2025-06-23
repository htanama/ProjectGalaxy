/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/28/2025
    Date Updated: 02/01/2025
    Description: Triggers the next mission in the queue if the criteria is met
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective7 : MonoBehaviour
{
    [SerializeField] GameObject boss;
    [SerializeField] Transform bossSpawn;
    [SerializeField] GameObject[] walls;
    [SerializeField] GameManager entranceCollider;
    [SerializeField] GameObject bossBar;

    string objectiveID = "7";

    bool playerInRange;

    private void Update()
    {
        if (playerInRange && SceneManagerScript.instance.SaveData.energyCellsCollected >= 3)
        {
            if (!SceneManagerScript.instance.SaveData.IsObjectiveCompleted(objectiveID))
            {
                GameObject boss = GameObject.FindWithTag("Boss");

                boss.GetComponent<Boss>().enabled = true;
                boss.GetComponent<MeshRenderer>().enabled = true;
                boss.GetComponent<CapsuleCollider>().enabled = true;

                //Instantiate(boss, bossSpawn.position, Quaternion.identity);     //spawn boss
                foreach (GameObject wall in walls) { wall.SetActive(true); }    //activate walls

                bossBar.SetActive(true);

                entranceCollider.enabled = true;

                ObjectiveManager.instance.CompleteObjective();

                SceneManagerScript.instance.MarkObjectAsDestroyed(this.GetComponent<UniqueID>().ID);

                //mark as complete
                SceneManagerScript.instance.SaveData.MarkObjectiveAsCompleted(objectiveID);
                SceneManagerScript.instance.SaveGame();     //save progress

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
