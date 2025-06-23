using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class HealthEvent : MonoBehaviour
{
    public HealthSystem healthSystem;    

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            healthSystem.Damage(1);
            Debug.Log("Taking Damage");
        }
    }

}
