/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/28/2025
    Date Updated: 02/01/2025
    Description: Triggers the next mission in the queue if the criteria is met
 */
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Objective2 : MonoBehaviour
{
    [SerializeField] GameObject firstCell;

    List<GameObject> enemiesAlive = new List<GameObject>();     //track enemies alive

    string objectiveID = "2";

    private void Start()
    {
        //count enemies in the area
        Collider[] colliders = Physics.OverlapSphere(transform.position, this.GetComponent<SphereCollider>().radius);
        foreach (Collider c in colliders)
        {
            if (c.CompareTag("Enemy"))      //checks for enemies
            {
                enemiesAlive.Add(c.gameObject);
            }
        }
    }

    private void Update()
    {
        if (!SceneManagerScript.instance.SaveData.IsObjectiveCompleted(objectiveID))
        {
            //update enemies killed
            enemiesAlive.RemoveAll(enemy => enemy == null);

            if (enemiesAlive.Count == 0)
            {
                ObjectiveManager.instance.CompleteObjective();
                firstCell.SetActive(true);

                //mark as complete
                SceneManagerScript.instance.SaveData.MarkObjectiveAsCompleted(objectiveID);
                SceneManagerScript.instance.SaveGame();     //save progress

                Destroy(gameObject);
            }
        }
    }
}
