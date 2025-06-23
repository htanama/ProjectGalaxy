/*
    Author: Juan Contreras
    Edited By:
    Date Created: 01/15/2025
    Date Updated: 01/15/2025
    Description: Abstract class to use when creating scriptable objects for items to be
                 picked up in the game scene(s).
 */
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ItemBase : ScriptableObject
{
    [SerializeField] protected string itemName;
    [SerializeField] protected Sprite icon;         //for inventory UI
    [SerializeField] protected ItemType itemType;
    [SerializeField] protected int maxStackSize = 1;
    [SerializeField] protected int value;   //how much that single pick up is worth
    [Header("MODEL PREFAB NEEDED HERE TO INSTANTIATE")]
    [SerializeField] protected GameObject itemModel;

    [Header("INVENTORY UI")]
    [SerializeField] Image invImage;
    [SerializeField] TextMeshProUGUI invName;
    [SerializeField] TextMeshProUGUI invNumText;
    [SerializeField] TextMeshProUGUI invMaxText;

    public string ItemName => itemName;
    public int MaxStackSize => maxStackSize;
    public GameObject ItemModel => itemModel;
    public ItemType ItemType_ => itemType;
    public Sprite Icon => icon;
    public enum ItemType
    {
        Weapon,
        Ammo,
        Collectible,
        MissionItem,
        Health,
    }

    public abstract void IntendedUse();
}
