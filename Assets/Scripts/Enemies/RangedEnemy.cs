/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/19/2025
    Date Updated: 01/25/2025
    Description: Logic for the ranged enemy, basic behavior
 */
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class RangedEnemy : EnemyBase
{
    [Header("     Ranged Enemy Variables     ")]
    [SerializeField] int roamDist;  //sphere distance of roaming
    [SerializeField] int roamTimer; //how long to wait before move again

    /*private float shootRate;
    private int currentAmmo;
    private int maxAmmo;
    private int shootDistance;
    private int reloadRate;*/

    Vector3 startingPos;

    bool playerInSight;
    bool isRoaming;

    protected override void Start()
    {
        base.Start();

        startingPos = transform.position; //to remember the starting position for roaming
        animator = this.GetComponent<Animator>();
            weaponInAction.EquipWeapon(0);

        // if this is an override, it's not calling the base start
        //GameManager.instance.OnGameStateChange += OnGameStateChange;
    }

    void Update()
    {
        //EnemyHPBar.fillAmount = (float)currentHealth / maxHealth;
        Behavior();
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    protected override void Behavior()
    {
        if (player == null || this == null) return;

        if(playerInSight)
        {
            StopCoroutine(RoamRoutine());
            base.HandleWeapon();
        }
        else if (!isRoaming && !playerInSight)
        {
            StartCoroutine(RoamRoutine());
        }
    }

    private void OnTriggerEnter(Collider other)     //player entering line of sight
    {
        if (other == null) return;

        if (other.gameObject.CompareTag("Player"))
        {
            playerInSight = true;
        }
    }

    private void OnTriggerExit(Collider other)      //player exiting line of sight
    {
        if (other == null) return;

        if(other.gameObject.CompareTag("Player"))
        {
            playerInSight = false;
        }
    }

    public override void TakeDamage(float amount)      //All enemies take damage
    {
        base.TakeDamage(amount);
    }

    // Enemy Roaming //
    IEnumerator RoamRoutine()
    {
        // turn on 
        isRoaming = true;

        //only for roaming to make sure the AI reaches destination
        agent.stoppingDistance = 0;

        //how big is our roaming distance 
        Vector3 randomPos = UnityEngine.Random.insideUnitSphere * roamDist;
        randomPos += startingPos;

        //enemy is Hit by Player
        NavMeshHit hit; //get info using similar like raycast
        NavMesh.SamplePosition(randomPos, out hit, roamDist, 1); //remember where the hit is at. 
        agent.SetDestination(hit.position); //player last known position

        //IEnums must have yield
        yield return new WaitForSeconds(roamTimer); // wait for second before continuing. 

        //turn off
        isRoaming = false;
    }

    //FOR PAUSE
    //private void OnGameStateChange(GameState newGameState)
    //{
    //    if (newGameState == GameState.Pause)
    //    {
    //        this.enabled = false;
    //    }
    //    else if (newGameState == GameState.Gameplay)
    //    {
    //        this.enabled = true;
    //    }
    //}
    //private void OnDestroy()
    //{
    //    // Unsubscribe
    //    GameManager.instance.OnGameStateChange -= OnGameStateChange;
    //}
}
