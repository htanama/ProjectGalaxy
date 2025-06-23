/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/17/2025
    Date Updated: 01/27/2025
    Description: Logic for how the boss interacts with the player depending on the encounter.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Unity.VisualScripting;

public class Boss : MonoBehaviour
{
    int currentEncounter = 1;
    List<IBossAbility> abilities = new List<IBossAbility>();

    Transform player;
    NavMeshAgent agent;

    [Header("MOVEMENT SETTINGS")]
    [SerializeField] [Range(1, 20)] int moveSpeed = 3;         //walking speed of the boss
    [SerializeField] [Range(15, 50)] int chargeSpeed = 20;       //speed when charging at the player
    [SerializeField] [Range(5, 20)] int keepDistance = 10;     //distance to keep from the player       (use stopping distance?)
    [SerializeField] [Range(1, 30)] int chargeCooldown = 10;   //time between charges
    [SerializeField] [Range(1, 5)] int chargeStopDistance = 2; //distance for the boss to stop at when charging
    [SerializeField] GameObject bossHealthBar;

    bool isCharging;
    bool isMoving;
    Coroutine movementRoutine;

    //new
    bool isGroundAttacking;

    public Transform Player => player;
    public NavMeshAgent Agent => agent;

    //Unity Events to notify when each ability is activated for animation and sound

    // Start is called before the first frame update
    public void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        agent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            Debug.LogError("Boss: Player not found.");
        }

        //// Subscribe to the State Changes
        //if(GameManager.instance != null)
        //    GameManager.instance.OnGameStateChange += OnGameStateChange;

        if (agent != null)
        {
            agent.speed = moveSpeed;
            SetupAbilities();           //double call in set next encounter method
            StartMovementBehavior();
        }
    }

    void Update()
    {
        if ((Vector3.Distance(player.transform.position, this.transform.position) < 40))
            StartMovementBehavior();
    }

    public void AddAbility(IBossAbility ability)
    {
        abilities.Add(ability);
        ability.Initialize(this);
    }
    //the pattern in which the boss activates the abilities
    public void ActivateAbilities()
    {
        //logic to when to activate abilities (not done)
        foreach (IBossAbility ability in abilities)
        {
            if(!isGroundAttacking)  //prevent attacks while charging
                ability.Execute();
        }
    }
    //called when boss is defeated
    public void SetupNextEncounter()
    {
        currentEncounter++;
        SetupAbilities();
    }

    public void SetupAbilities()
    {
        //setup in case there is time to add more abilities
        switch(currentEncounter)
        {
            case 1:
                FindAbility("ChargedLaser");
                FindAbility("GroundAttack");
                break;
            case 2:
                //AddAbility();
                //AddAbility();
                break;
            case 3:
                //AddAbility();
                //AddAbility();
                break;
        }
    }

    //may be able to remove upon refactor
    public void FindAbility(string abilityName)
    {
        Transform abilityTransform = transform.Find(abilityName);
        if (abilityTransform != null)
        {
            //iterate to find active abilities
            IBossAbility ability = abilityTransform.GetComponent<IBossAbility>();
            if (ability != null)
            {
                AddAbility(ability);
                abilityTransform.gameObject.SetActive(true);
            }
            else
                Debug.Log($"Ability {abilityName} is not found.");
        }
        else
            Debug.Log($"Ability transform for {abilityName} not found.");
    }

    public void StartMovementBehavior()
    {
        //calling the movement coroutine
        if(movementRoutine == null && !isMoving)
            movementRoutine = StartCoroutine(MovementBehavior());
    }

    IEnumerator MovementBehavior()
    {
        isMoving = true;

        while (true)
        {
            if ((Vector3.Distance(player.transform.position, this.transform.position) < 40))
            {
                if (bossHealthBar != null && !bossHealthBar.activeSelf)
                    bossHealthBar.SetActive(true);
                
                    Debug.Log("Boss: HP bar not found");

                if (!isCharging)
                {
                    MaintainDistance();

                    //charge after cooldown
                    //yield return new WaitForSeconds(chargeCooldown);

                    //another check in case bool changes while on cooldown from other coroutine (might not need)
                    if (!isGroundAttacking)
                    {
                        //StartCoroutine(ChargeAtPlayer());
                        StartGroundAttack();
                    }
                    else
                    {
                        ActivateSpecificAbility<ChargedLaser>();

                        yield return new WaitForSeconds(2);
                    }
                }
            }
            isMoving = false;

            yield return null;
        }
    }

    public void MaintainDistance()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > keepDistance + 1f) //with adjustment for natural-like movement
        {
            //head to the player when far
            agent.isStopped = false;                                //don't need?
            agent.SetDestination(player.position);
            Debug.Log("Boss: Moving closer to the player.");
        }
        else if (distanceToPlayer < keepDistance - 1f)
        {
            Debug.Log("Boss: Moving away from player");

            agent.isStopped = false;
            //move away to keep distance
            Vector3 directionAway = (transform.position - player.position).normalized;
            Vector3 destination = transform.position + directionAway * keepDistance;
            agent.SetDestination(destination);
        }
    }

    //just found out this is a thing
    public void ActivateSpecificAbility<T>()where T : IBossAbility
    {
        foreach (IBossAbility ability in abilities)
        {
            if(ability is T)
            {
                ability.Execute();
            }
        }
    }


    //can go into GroundAttack interface
    IEnumerator ChargeThenGroundAttack()
    {
        //charge at player
        isCharging = true;
        agent.isStopped = false;
        agent.speed = chargeSpeed;

        Vector3 targetPosition = player.position;
        agent.SetDestination(targetPosition);

        while (Vector3.Distance(transform.position, player.position) > chargeStopDistance)
        {
            Debug.Log("Boss: Charging at player");
            //keep updating position to chase even if player is moving
            targetPosition = player.position;
            agent.SetDestination(targetPosition);
            //wait for next frame
            yield return null;
        }

        //reset
        isCharging = false;
        agent.speed = moveSpeed;

        //start ground attack
        Debug.Log("Boss: Starting GroundAttack");
        ActivateSpecificAbility<GroundAttack>();

        //reset state after ground attack

        agent.isStopped = false;
        StartMovementBehavior();

        //ground attack cool down
        yield return new WaitForSeconds(chargeCooldown);

        isGroundAttacking = false;
    }


    public void StartGroundAttack()
    {
        if (!isGroundAttacking)
        {
            Debug.Log("Boss: Starting GroundAttack");

            isGroundAttacking = true;

            StopAllAbilities();

            StartCoroutine(ChargeThenGroundAttack());
        }
    }

    void StopAllAbilities()
    {
        //stop whatever he is doing
        agent.isStopped = true;
        Debug.Log("Boss: Stopping all abilities for GroundAttack");
    }


    ////FOR PAUSE
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
