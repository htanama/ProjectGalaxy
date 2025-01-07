using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickup
{
    public void WeaponPickup(Collider collideWithItems);
    public void AmmoPickup(Collider collideWithItems);
    public void HealthPackPickUp(Collider collideWithItems);
}