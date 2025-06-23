using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstaDeath : MonoBehaviour
{
    // Instant kill on entrance of death FOG
    public void OnTriggerEnter(Collider other)
    {
        HealthSystem playerHealth = other.GetComponent<HealthSystem>();

        if(other.CompareTag("Player") && playerHealth != null)
        {
            playerHealth.Damage(playerHealth.MaxHealth * 10000000000000);
        }
    }
}
