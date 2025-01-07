using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour, IPickup
{
    private void OnTriggerEnter(Collider collideWithItems)
    {
        HealthPackPickUp(collideWithItems);
        WeaponPickup(collideWithItems);
        AmmoPickup(collideWithItems);
    }

    public void HealthPackPickUp(Collider collideWithItems)
    {
        if (collideWithItems.CompareTag("Health Pickup"))
        {
            // implement healing stuff
            Debug.Log("player collided with Health Pack");
        }
        else
        {
            return;
        }

    }

    public void WeaponPickup(Collider collideWithItems)
    {
        if (collideWithItems.CompareTag("Weapon Pickup"))
        {
            //implement pick up stuff
            Debug.Log("player collided with Weapon");
        }
        else
        {
            return;
        }
    }
    public void AmmoPickup(Collider collideWithItems)
    {
        if (collideWithItems.CompareTag("Ammo Pickup"))
        {
            //implement ammo pickup
            Debug.Log("player collided with Ammo");
        }
        else
        {
            return;
        }
    }

}
