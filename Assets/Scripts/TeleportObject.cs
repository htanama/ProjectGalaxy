using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportObject : MonoBehaviour
{
    public Transform destination;    
    public GameObject objectTeleported;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            objectTeleported.SetActive(false);

            objectTeleported.transform.position = 
                new Vector3 (destination.position.x + 40, destination.position.y, destination.position.z);

            objectTeleported.SetActive(true);
        }
    }
}
