using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TakingAmmo : MonoBehaviour
{
    public static event Action OnTakingAmmo;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnTakingAmmo?.Invoke();             
        }
    }
}
