/*
    Author: Juan Contreras
    Edited By:
    Date Created: 01/15/2025
    Date Updated: 01/16/2025
    Description: Class to use when creating scriptable objects for collectibles to be
                 picked up in the game scene(s).
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCollectible", menuName = "Inventory/Collectible")]        //for now, will most likely move to derived classes only
public class Collectible : ItemBase
{

    public override void IntendedUse()
    {

    }
}
