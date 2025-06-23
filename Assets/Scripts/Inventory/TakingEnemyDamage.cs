using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;


// put the on the emeny object to take damagae by calling HealthSystem
public class TakingEnemyDamage : MonoBehaviour
{
    public UnityEvent<float> OnTakingDamage;             

    void Start()
    {
        // subscription
        // WeaponInAction.OnGettingHit += WeaponInAction_OnGettingHit;
    }

    private void WeaponInAction_OnGettingHit()
    {
        OnTakingDamage?.Invoke(1);
    }
   
}
