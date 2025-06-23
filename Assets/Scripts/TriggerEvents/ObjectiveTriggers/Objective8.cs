/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/28/2025
    Date Updated: 02/01/2025
    Description: Triggers the next mission in the queue if the criteria is met
 */
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

public class Objective8 : MonoBehaviour
{
    [SerializeField] GameObject[] walls;
    [SerializeField] GameObject shipEntranceCollider;

    HealthSystem bossHealth;

    string objectiveID = "8";
    

    private void Start()
    {
        if (!SceneManagerScript.instance.SaveData.IsObjectiveCompleted(objectiveID))
            StartCoroutine(WaitForBoss());
    }

    IEnumerator WaitForBoss()
    {
        GameObject boss = null;

        //wait until the boss is spawned in
        while (boss == null)
        {
            boss = GameObject.FindWithTag("Boss");

            if(boss != null )
            {
                bossHealth = boss.GetComponent<HealthSystem>();

                if(bossHealth != null )
                {
                    bossHealth.OnDeath.AddListener(OnBossDefeated);     //subscribe to detect when killed
                }
            }

            yield return null;      //keeps checking next frame
        }
    }

    private void OnBossDefeated()
    {
        ObjectiveManager.instance.CompleteObjective();

        //mark as complete
        SceneManagerScript.instance.SaveData.MarkObjectiveAsCompleted(objectiveID);
        SceneManagerScript.instance.MarkObjectAsDestroyed(this.GetComponent<UniqueID>().ID);
        SceneManagerScript.instance.MarkObjectAsDestroyed(shipEntranceCollider.GetComponent<UniqueID>().ID);

        Destroy(shipEntranceCollider);

        foreach (GameObject wall in walls)
        {
            SceneManagerScript.instance.MarkObjectAsDestroyed(wall.GetComponent<UniqueID>().ID);
            Destroy(wall);
        }

        SceneManagerScript.instance.SaveGame();     //save progress
        Destroy(gameObject);
    }
}
