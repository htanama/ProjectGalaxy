using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TakingPlayerDamage : MonoBehaviour
{
    public UnityEvent<float> OnTakingDamage;
    
    private void Start()
    {
        //RangedEnemy.OnShootingPlayer += RangedEnemy_OnShootingPlayer;
    }

    private void RangedEnemy_OnShootingPlayer()
    {
        OnTakingDamage?.Invoke(1);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnTakingDamage?.Invoke(30);
            Debug.Log("Damage 30 HP");
        }
    }
}
