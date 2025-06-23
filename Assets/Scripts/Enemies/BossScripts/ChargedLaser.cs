/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/17/2025
    Date Updated: 01/18/2025
    Description: Interface for the charged laser ability for the boss
 */
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class ChargedLaser : MonoBehaviour, IBossAbility
{
    //effects and animations needed
    /*
    - charge effect
    - laser effect
    - charge sound
    - ShootRoutine sound
    - laser beam asset
    */
    public UnityEvent<float> OnDamage;


    [Header("LASER INTERACTION SETTINGS")]
    [SerializeField] [Range(0.5f, 5f)] float chargeTime = 2.0f;             //time to charge the laser
    [SerializeField] [Range(0.3f, 3f)] float lockOnShootDelay = 1.0f;       //delay to ShootRoutine once boss locks on
    [SerializeField][Range(1f, 10f)] float aimRotationSpeed = 5.0f;         //how fast to turn and look at the player when aiming the laser

    [Header("MORE LASER SETTINGS")]
    [SerializeField] LineRenderer laserBeam;
    [SerializeField] [Range(0.1f, 10.0f)] float laserWidth = 0.5f;
    [SerializeField] [Range(0.1f, 2.0f)] float laserDuration = 0.5f;
    [SerializeField] GameObject warningIndicatorPrefab;
    [SerializeField] [Range(1, 20)] int damageAmount = 10;

    Boss boss;
    Vector3 lockedShootPos;

    GameObject warningIndicatorInstance;

    public void Initialize(Boss boss )
    {
        this.boss = boss;
    }

    public void Execute()
    {
        Debug.Log("Charging laser ability");

        if (boss.Player != null)
        {
            boss.StartCoroutine(ChargeLaserRoutine());
        }
        else
            Debug.Log("Boss(ChargedLaser): No player reference found!");
    }

    IEnumerator ChargeLaserRoutine()
    {
        //charge and aim laser
        float timer = 0f;
        while(timer < chargeTime)
        {
            timer += Time.deltaTime;

            //boss aims at player while charging
            AimAtPlayer();

            yield return null;
        }

        //after laser is charged, lock position to ShootRoutine
        lockedShootPos = boss.Player.position;

        //create warning indicator before shooting
        if(warningIndicatorPrefab != null)
        {
            warningIndicatorInstance = UnityEngine.Object.Instantiate(warningIndicatorPrefab, lockedShootPos, Quaternion.identity);
        }

        Debug.Log($"Laser locked at {lockedShootPos}");

        yield return new WaitForSeconds(lockOnShootDelay);

        //destroy indicator right before firing
        if(warningIndicatorInstance != null) { UnityEngine.Object.Destroy(warningIndicatorInstance); }

        //ShootRoutine
        ShootLaser(lockedShootPos);
    }

    void AimAtPlayer()
    {
        Transform player = boss.Player;
        if (player != null)
        {
            //turn to look at the player
            Vector3 directionToPlayer = player.position - boss.transform.position;
            //directionToPlayer.y = 0;                                              //use if boss is jittery
            boss.transform.rotation = Quaternion.Lerp(
                boss.transform.rotation,
                Quaternion.LookRotation(directionToPlayer),
                Time.deltaTime * aimRotationSpeed);
        }
    }

    void ShootLaser(Vector3 targetPosition)
    {
        Debug.Log($"Boss: Firing laser at {targetPosition}");
        //ShootRoutine laser logic
        if(laserBeam != null)
        {
            //start and end of laser
            laserBeam.SetPosition(0, boss.transform.position);
            laserBeam.SetPosition(1, targetPosition);

            //setting laser width
            laserBeam.startWidth = laserWidth;
            laserBeam.endWidth = laserWidth;

            //show laser for the set duration
            laserBeam.enabled = true;
            boss.StartCoroutine(DisableLaserBeam());

            //call shoot sound?
        }

        //check if player is in laser damage range
        if(Vector3.Distance(targetPosition, boss.Player.position) <= laserWidth)
        {
            Debug.Log("Player hit by laser");

            //damage the player
            //call TakeDamage(damageAmount) on player
            //OnDamage?.Invoke(damageAmount);     //Health System

            boss.Player.GetComponent<HealthSystem>().Damage(damageAmount);
        }
    }

    IEnumerator DisableLaserBeam()
    {
        Debug.Log("Disabling laser.");
        //turn off the beam after a set duration
        yield return new WaitForSeconds(laserDuration);
        laserBeam.enabled = false;
    }
}
