/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/19/2025
    Date Updated: 01/24/2025
    Description: Core functionality of the stun enemy, takes an inventory item and runs away
 */
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class StunEnemy : EnemyBase
{
    [Header("     Stun Enemy Stats     ")]
    [SerializeField] float stunDuration = 4;
    [SerializeField] int distanceFromPlayer = 4;    //how close to get to the player to take the item
    [SerializeField] float stunSensitivity = 10;      //how much to slow the camera down during the stun
    [SerializeField] int fleeDistance = 40;      //distance to keep from player (might not need)
    [SerializeField] int roamRadius = 20;

    //for interactions with the player
    //GameObject player;
    GameObject itemModel;       //to attach model to enemy
    //playerScript playerSettings;
    enum EnemyState { Roaming, Chasing, Fleeing }            //Behavior changes when taking item
    EnemyState currentState = EnemyState.Roaming;   //Starts by roaming

    Vector3 roamPosition;

    bool isRoaming;
    bool isFleeing;     //Used to set speed once
    bool isInventoryEmpty;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //Initializing stats
        agent.speed *= speed;
        agent.stoppingDistance = distanceFromPlayer;
        roamPosition = agent.destination;
        animator = this.GetComponent<Animator>();

        // Subscribe to the State Changes
        //GameManager.instance.OnGameStateChange += OnGameStateChange;
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerInventory();
        if (!isInventoryEmpty)        //will only go after player if inventory is not empty
            currentState = EnemyState.Chasing;

        Behavior();     //the way the enemy acts around the player
    }

    public override void TakeDamage(float amount)
    {
        //drop item right before dying
        if (healthSystem.CurrentHealth - amount <= 0)
        {

            if (itemModel != null)
            {
                //drop item logic
                itemModel.transform.SetParent(null);     //Detach item from carrier
                itemModel.transform.position = transform.position; //Drop item at enemy's death location
                itemModel.GetComponent<Collider>().enabled = true;   //Enable item collider for pickup
            }
        }
        //call damage method (handles death)
        base.TakeDamage(amount);
        Debug.Log($"Stun Enemy: Took {amount} damage");
        //use unity event here
    }

    protected override void Behavior()
    {
        switch (currentState)
        {
            case EnemyState.Roaming:
                if (!isRoaming)
                    StartCoroutine(RoamRoutine()); ;
                break;
            case EnemyState.Chasing:
                ChasePlayer();
                break;
            case EnemyState.Fleeing:
                FleePlayer();
                break;
        }
    }

    void Roam()
    {
        isRoaming = true;
        //find a random location within the RoamRoutine radius
        Vector3 randomLocation = Random.insideUnitSphere * roamRadius;
        randomLocation += transform.position;

        //find a valid point on the NavMesh to go to
        NavMeshHit hit;
        if(NavMesh.SamplePosition(randomLocation, out hit, 50, NavMesh.AllAreas))      //maybe higher max distance if there's issues
        {
            if (Vector3.Distance(hit.position, transform.position) > 10)
            {
                roamPosition = hit.position;
                agent.SetDestination(roamPosition);
            }
        }
    }

    IEnumerator RoamRoutine()
    {
        if(!isRoaming)
            Roam();

        if (agent.destination == roamPosition)
        {
            isRoaming = false;
            yield return new WaitForSeconds(2);
        }
    }

    void ChasePlayer()
    {
        if(isInventoryEmpty)
        {
            isRoaming = false;                      //early exit if player inventory becomes empty when chasing
            currentState = EnemyState.Roaming;
            //StopCoroutine(RoamRoutine());
        }

        Debug.Log("Stun Enemy: Chasing after player");

        //move to player location anywhere on the scene when the player is within range
        agent.SetDestination(player.transform.position);
        //stun and take item from player
        if (Vector3.Distance(transform.position, player.transform.position) < agent.stoppingDistance)
        {
            if (!isInventoryEmpty)
            {
                StunPlayer();               //stuns the player
                TakeItemFromPlayer();        //takes item and flees
            }
        }
    }

    void FleePlayer()
    {
        //enemy runs faster
        if (!isFleeing)
        {
            agent.speed = (playerSettings.Speed *
                playerSettings.SprintMod) - 1;       //He runs slightly slower than player sprint speed

            //Prevent speed from being set more than once
            isFleeing = true;
        }

        //find direction away from player
        Vector3 playerPosition = player.transform.position;
        Vector3 fleeDirection = (transform.position - playerPosition).normalized;

        //adding randomness to flee direction
        fleeDirection += new Vector3(Random.Range(-0.2f, 0.2f), 0, Random.Range(-0.2f, 0.2f));
        fleeDirection.Normalize();

        //destination to run to
        Vector3 fleeDestination = (transform.position + fleeDirection) * fleeDistance;

        //making sure a valid destination is posHit to prevent running into walls or side of the map(NEEDS FIX)
        NavMeshHit posHit;
        NavMeshHit edgeHit;
        if (NavMesh.SamplePosition(fleeDestination, out posHit, 100, NavMesh.AllAreas))
        {
            //move to valid point
            if (Vector3.Distance(posHit.position, transform.position) > 0)
                agent.SetDestination(posHit.position);
            else
                Debug.Log($"Stun Enemy: Destination found but not set at {posHit.position}. Player Position: {player.transform.position}");

            if (Vector3.Distance(posHit.position, transform.position) > 10)
            {
                if (agent.FindClosestEdge(out edgeHit))
                {
                    if (Vector3.Distance(edgeHit.position, transform.position) < 5)
                    {
                        //calculate a direction away from the edge
                        Vector3 awayFromEdge = (transform.position - edgeHit.position).normalized;

                        //adjust the RoamRoutine position to move away from the edge
                        Vector3 adjustedPosition = transform.position + awayFromEdge * 5; //move farther??
                        NavMeshHit adjustedHit;

                        //validate the new position on the NavMesh
                        if (NavMesh.SamplePosition(adjustedPosition, out adjustedHit, 10, NavMesh.AllAreas))
                        {
                            roamPosition = adjustedHit.position;
                            agent.SetDestination(roamPosition);
                            Debug.Log("Avoiding edge by moving to: " + roamPosition);
                        }
                    }
                    roamPosition = posHit.position;
                    agent.SetDestination(roamPosition);
                }
            }
        }
        else
            Debug.Log("Stun Enemy: Invalid flee destination");

        //move to that destination
        //agent.SetDestination(fleeDestination);
    }

    void StunPlayer()
    {
        Debug.Log($"Stun Enemy: Stunning player for {stunDuration} seconds");

        //stun player for set duration and change sensitivity during stun
        playerSettings.Stun(stunDuration, stunSensitivity);
    }

    void TakeItemFromPlayer()
    {
        Debug.Log("Taking item from player");
        
        //creating random index to choose item
        int randomIndex = Random.Range(0, InventoryManager.instance.InventorySlotsList.Count);
        InventorySlot itemSlot = InventoryManager.instance.InventorySlotsList[randomIndex];

        GameObject currentHeldItem = GameObject.FindWithTag("CarryingSlot");    //TEMPORARY (Maybe)

        //attach item model to enemy
        if (itemSlot.Item.ItemModel != null)
        {
            itemModel = Instantiate(itemSlot.Item.ItemModel, transform);
            itemModel.transform.localPosition = new Vector3(0, 1.5f, 0);                   //location of item model on enemy (adjust as needed)
            itemModel.transform.localRotation = Quaternion.identity;
            itemModel.GetComponent<Collider>().enabled = false;     //disable collider so player has to defeat to take item back

            Debug.Log($"Stun Enemy: {itemSlot.Item} taken and model attached");

            if (itemSlot.Item.ItemModel.GetComponent<MeshFilter>().sharedMesh == currentHeldItem.GetComponent<MeshFilter>().sharedMesh)
            {
                currentHeldItem.GetComponent<MeshFilter>().sharedMesh = null;           //removing physical gun in hand
                currentHeldItem.GetComponent<MeshRenderer>().sharedMaterial = null;

                //currentHeldItem.GetComponent<WeaponInAction>().GunInfo = null;          //clearing info of currently held gun (Now taken care of by WeaponInAction)

                player.GetComponent<WeaponInAction>().CheckAvailableWeapons();          //update weapon inventory
            }

            //remove item from player inventory
            InventoryManager.instance.OnDrop(itemSlot.Item, 1);

            currentState = EnemyState.Fleeing;  //Change enemy state when taking the item
        }
        else
            Debug.Log("Stun Enemy: No Model found for stolen item");

        //UI changes?   Taken care of by InventoryManager
    }

    void CheckPlayerInventory()
    {
        isInventoryEmpty = true;

        //checks if the player's inventory is empty
        if (InventoryManager.instance != null)
        {
            if (InventoryManager.instance.InventorySlotsList.Count > 0)
                isInventoryEmpty = false;
            //else
               // Debug.Log("Stun Enemy: Player Inventory Empty");
        }
        else
            Debug.Log("Stun Enemy: No Inventory Manager Instance");
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
