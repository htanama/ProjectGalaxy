/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/17/2025
    Date Updated: 01/17/2025
    Description: Interface for the abilities to be added onto the boss
                 with each encounter.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBossAbility
{
    void Initialize(Boss boss);     //initialize the boss with the ability
    void Execute();                 //logic for specific ability
}
