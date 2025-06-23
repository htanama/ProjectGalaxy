/*
    Author: Juan Contreras
    Edited By:
    Date Created: 01/15/2025
    Date Updated: 01/16/2025
    Description: Weapon base class scriptable objects for the player to use and store in inventory
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Inventory/Weapon")]
public class WeaponTest : ItemBase
{
    [SerializeField] private int damage;
    
    //read-only getter
    public int Damage => damage;

    public override void IntendedUse()
    {
        //equipping
    }
}
