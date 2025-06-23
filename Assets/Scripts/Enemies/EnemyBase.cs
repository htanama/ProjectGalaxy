/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/19/2025
    Date Updated: 01/23/2025
    Description: Abstract class for all enemies
 */
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("BASE ENEMY STATS")]
    [SerializeField] protected NavMeshAgent agent;      //Components shared between all/most enemy types
    [SerializeField] protected int speed;
    [SerializeField] protected Renderer model;
    [SerializeField] protected Animator animator;
    //[SerializeField] protected float maxHealth;

    [Header("WEAPON AND TARGETING")]
    [SerializeField] protected WeaponInAction weaponInAction;     //weapon system for weapon in use
    [SerializeField] protected TargetingSystem targetingSystem;   //targeting system for enemies
    [SerializeField] protected HealthSystem healthSystem;   //targeting system for enemies
    [SerializeField] int enemyShootRate;

    protected GameObject player;
    protected playerScript playerSettings;
    protected UniqueID uniqueID;

    //getters and setters
    //public float CurrentHealth
    //{
    //    get { return currentHealth; }
    //    set { currentHealth = value; }
    //}

    public int EnemyShootRate => enemyShootRate;

    protected virtual void Start()
    {
        uniqueID = GetComponent<UniqueID>();

        if(SceneManagerScript.instance.SaveData.destroyedObjects.Contains(uniqueID.ID))
        { Destroy(gameObject); }        //destroy if previously killed

        if (GameManager.instance != null)
        {
            player = GameObject.FindWithTag("Player");
            playerSettings = player.GetComponent<playerScript>();
        }

        if (this.GetComponent<TargetingSystem>() != null)
            this.GetComponent<SphereCollider>().radius = targetingSystem.DetectionRadius;
    }

    //To be defined in each enemy class
    protected abstract void Behavior();     //For consistency and clarity           //in update with HandleWeapon

    //handle weapon logic for the enemy holding the weapon (AI)
    protected void HandleWeapon()
    {
        targetingSystem.AimAtTarget();

        if (targetingSystem.CurrentTarget != null)
        {
            //look at target
            Vector3 direction = targetingSystem.CurrentTarget.position - transform.position;
            direction.y = 0;                                                                    //keeps turning horizontal only (might delete)
            transform.rotation = Quaternion.LookRotation(direction);

            //shoot while gun has ammo in the clip
            if (weaponInAction.CurrentAmmo > 0)
            {
                //shoot
                weaponInAction.EnemyFireGun(targetingSystem.CurrentTarget);
            }
            else
            {
                //reload
                weaponInAction.Reload();
            }
        }
    }

    public virtual void TakeDamage(float amount)      //All enemies take damage
    {
        animator.SetTrigger("isHit");
        healthSystem.CurrentHealth -= amount;

        if (healthSystem.CurrentHealth <= 0)
        {
            animator.SetTrigger("isDead");

            SceneManagerScript.instance.MarkObjectAsDestroyed(uniqueID.ID);
            SceneManagerScript.instance.SaveGame();

            if (weaponInAction)
                weaponInAction.DropEquippedGun();

            Destroy(gameObject);        //Dead
        }
    }

}
