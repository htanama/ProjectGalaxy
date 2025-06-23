/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/18/2025
    Date Updated: 01/19/2025
    Description: Interface for the ground attack ability for the boss
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GroundAttack : MonoBehaviour, IBossAbility
{
    public UnityEvent<float> OnDamage;

    Boss boss;

    [Header("DAMAGE WAVE SETTINGS")]
    [SerializeField] GameObject wavePrefab;
    [SerializeField][Range(5f, 20f)] float maxRadius = 10;
    [SerializeField][Range(5f, 20f)] float innerSafeRadius = 8;      //area inside the attack where the player is unaffected
    [SerializeField] int expansionSpeed = 5;
    [SerializeField] float damageAmount = 10;
    [SerializeField][Range(0.5f, 3.0f)] float fadeDuration = 2f;

    Vector3 attackCenter;    //to be set once in DamagePlayer()
    float distanceToPlayer;

    public void Initialize(Boss boss)
    {
        this.boss = boss;
    }

    public void Execute()
    {
        Debug.Log("Ground slam attack!");
        //logic for ground slam attack

        boss.StartCoroutine(GroundSlamRoutine());
    }

    IEnumerator GroundSlamRoutine()
    {
        //spawn position for the wave effect
        Vector3 spawnPos = this.transform.position;
        //spawnPos.y = 0f + boss.Agent.baseOffset;

        //start wave effect
        GameObject waveInstance = GameObject.Instantiate(wavePrefab, spawnPos, Quaternion.identity);

        //wave material to fade out
        Material waveMaterial = waveInstance.GetComponent<Renderer>()?.material;

        float currentRadius = 0f;
        float alpha = 1f;       //for fade

        float innerRadius = 0f;
        while (currentRadius <  maxRadius)
        {
            //damage the player
            DamagePlayer(currentRadius, innerRadius, spawnPos);

            //increase radius
            currentRadius += expansionSpeed * 2f * Time.deltaTime;
            waveInstance.transform.localScale = new Vector3(currentRadius, 1f, currentRadius);      //multiply by 2?

            yield return null;
        }
        //reset to null for next attack
        //attackCenter = null;

        //fade out effect
        float fadeTimer = 0f;
        while(fadeTimer < fadeDuration)
        {
            fadeTimer += Time.deltaTime;
            alpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeDuration);

            if(waveMaterial != null)
            {
                //update material transparency
                Color color = waveMaterial.color;
                color.a = alpha;
                waveMaterial.color = color;
            }

            yield return null;
        }

        //destroy wave effect
        GameObject.Destroy(waveInstance);
    }

    private void DamagePlayer(float currentRadius, float innerRadius, Vector3 attackCenter)
    {
        //increase inside at the same rate as outside once outside radius reaches desired size
        if((currentRadius - innerRadius) >= (maxRadius - innerSafeRadius))
            innerRadius = currentRadius - (maxRadius - innerSafeRadius);

        if (attackCenter != null)
        {
            //array since this returns an array and could not find one to return a single collider
            Collider[] outerHit = Physics.OverlapSphere(attackCenter, currentRadius);

            foreach (Collider hit in outerHit)
            {
                //check for the player
                if (hit.CompareTag("Player"))
                {
                    //calculate to see if player is in center (safe area)
                    float distanceToCenter = Vector3.Distance(attackCenter, hit.transform.position);

                    //check if player is outside inner safe area
                    if (distanceToCenter >= innerRadius)
                    {
                        Debug.Log("Player hit by ground attack");
                        //call player take damage with (damageAmount)
                        //OnDamage?.Invoke(damageAmount);
                        boss.Player.GetComponent<HealthSystem>().Damage(damageAmount);
                    }
                    else
                        Debug.Log("Player was not hit by ground attack");

                    Debug.Log($"if ({distanceToCenter} >= {innerRadius} Current radius: {currentRadius}");
                }
            }
        }
    }
}
