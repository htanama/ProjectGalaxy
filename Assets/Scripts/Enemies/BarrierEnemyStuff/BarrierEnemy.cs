/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/19/2025
    Date Updated: 01/19/2025
    Description: Class with core functionality for the barrier enemy
 */
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class BarrierEnemy : EnemyBase
{
    [Header("     Barrier Enemy Stats     ")]
    [SerializeField] private GameObject barrierObj;     //place barrier object here
    [SerializeField][Range(0.5f, 10.0f)] private float barrierCooldown;//time between barrier casts
    [SerializeField][Range(2.0f, 20.0f)] private float barrierLifetime;//how long the barrier is up for
    [SerializeField][Range(1.0f, 25.0f)] private float allyDetectionRadius;//radius to find enemy AI's
    [SerializeField][Range(1.0f, 10.0f)] private float followDistance;     //distance to stand behind enemy allies

    private float nextBarrierTime;          //time before allowing to create another barrier using cooldown time
    //set to infinity to guarantee first iteration of the condition is met
    private float closestDistance = Mathf.Infinity; //to temporarily hold distance of closest ally
    private float distance;     //holds distance of an ally to check if closer or not

    Transform closestAlly;      //keep track of closest enemy(ally)
    protected override void Start()
    {
        base.Start();

        weaponInAction.EquipWeapon(0);
        animator = this.GetComponent<Animator>();

        // Subscribe to the State Changes
        //GameManager.instance.OnGameStateChange += OnGameStateChange;
    }
    // Update is called once per frame
    void Update()
    {
        if(targetingSystem.CurrentTarget == null)
            targetingSystem.AimAtTarget();
        
        animator.SetFloat("Speed", agent.speed);

        Behavior();
    }

    //overriding from baseEnemy
    protected override void Behavior()
    {
        if(targetingSystem.CurrentTarget != null)
            HandleWeapon();
        else
            stayBehindEnemies();

        manageBarriers();
    }

    private void stayBehindEnemies()
    {
        //check for nearby enemies(allies) to support
        Collider[] alliesInRange = Physics.OverlapSphere(transform.position, allyDetectionRadius);

        foreach (Collider ally in alliesInRange)
        {
            //checking if colliders in range or not itself and only enemy types
            if (ally.gameObject != this.gameObject && ally.GetComponent<EnemyBase>() != null)
            {
                //current distance to an enemy(ally) in range
                distance = Vector3.Distance(transform.position, ally.transform.position);
                if (distance < closestDistance)
                {
                    closestAlly = ally.transform;       //closest ally found and stored
                    closestDistance = distance;         //stores closest's ally distance
                }
            }
        }

        //enemy stays behind closest ally
        if (closestAlly != null)
        {
            //find direction of the player
            Vector3 dirToPlayer = (player.transform.position - closestAlly.position).normalized;

            //find a position to stay behind the ally
            Vector3 posBehindAlly = closestAlly.position - dirToPlayer * followDistance;

            //move agent to that position
            agent.SetDestination(posBehindAlly);
        }
        else
            closestDistance = Mathf.Infinity;    //reset find distance if no enemies are found

    }

    private void manageBarriers()
    {
        //checks to see if the set time has passed to cast next barrier
        if (Time.time >= nextBarrierTime)
        {
            createBarrier();    //spawn barriers on allies
            nextBarrierTime = Time.time + barrierCooldown;  //sets time for next barrier cast
        }
    }

    private void createBarrier()            //add limit to one per enemy
    {
        //check for nearby enemies(allies) to support
        Collider[] alliesInRange = Physics.OverlapSphere(transform.position, allyDetectionRadius);

        foreach (Collider ally in alliesInRange)
        {
            //checking for allies to give barriers to
            CheckForAllies(ally);
        }
    }

    void CheckForAllies(Collider ally)
    {
        //checking if colliders in range are not itself and only enemy types
        if (ally.gameObject != this.gameObject && ally.GetComponent<EnemyBase>() != null)
        {
            //check if ally already has a barrier on them (limits one per ally)
            if (ally.GetComponentInChildren<barrier>() == null)
            {
                //assign barrier to an ally
                GiveBarrier(ally);
            }
        }
    }

    void GiveBarrier(Collider ally)
    {
        //instantiate a barrier object on the ally's position
        //ally.bounds.center creates the barrier at the center of the collider
        GameObject barrier = Instantiate(barrierObj, ally.bounds.center, Quaternion.identity);

        //attach barrier to ally to follow them
        barrier.transform.SetParent(ally.transform);

        //destroy barrier
        DestroyBarrier(barrier);
    }

    void DestroyBarrier(GameObject barrier)
    {
        //destroy barrier after lifetime is over
        if (barrierObj)
            Destroy(barrier, barrierLifetime);
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
