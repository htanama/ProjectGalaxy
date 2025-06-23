/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/19/2025
    Date Updated: 01/19/2025
    Description: Script to assign the object used as a barrier for the barrier enemy
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barrier : MonoBehaviour //, HealthSystem
{
    [SerializeField][Range(1.0f, 200f)] private float barrierHealth;            //incorporate with health system????

    public void TakeDamage(float amount)
    {
        barrierHealth -= amount;        //subtract damage taken from health

        //when health reaches zero
        if (barrierHealth <= 0)
        {
            //barrier destroyed
            Destroy(gameObject);
        }
    }
}
