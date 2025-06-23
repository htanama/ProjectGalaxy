using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GettingHealing : MonoBehaviour
{
    public UnityEvent<float> OnHealing;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnHealing?.Invoke(20);
            Debug.Log("Restore 20 HP");
        }
    }
}
