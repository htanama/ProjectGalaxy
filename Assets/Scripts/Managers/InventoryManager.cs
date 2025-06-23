/*
    Author: Juan Contreras
    Edited By
    Date Created: 01/16/2025
    Date Updated: 02/02/2025
    Description: Everything related to the player's inventory starts here

    Possible Edits: 
        - Add Sorting or separate lists by type (for UI)
        - Dropping or removing item from inventory (button press or from UI)
 */
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InventoryData
{
    public List<InventorySlotData> inventorySlots = new List<InventorySlotData>();
}

[System.Serializable]
public class InventorySlotData
{
    public string itemName;
    public int quantity;
}

public class InventoryManager : MonoBehaviour
{
    //singleton
    public static InventoryManager instance;

    GameObject player;

    //to use when loading inventory from save data
    public ItemBase[] itemsLoad;

    //Unity Event notifies Inventory was updated
    public UnityEvent OnInventoryUpdated;   //connect to CheckAvailable weapons in WeaponInAction

    //inventory storage
    List<InventorySlot> inventorySlots = new List<InventorySlot>();

    public List<InventorySlot> InventorySlotsList => inventorySlots;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); //keeps inventory between scenes

        }
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if(player == null)
            player = GameObject.FindWithTag("Player");
    }

    //handles item pickup
    public void OnPickup(ItemBase item, int quantity)
    {
        //Debug.Log($"InventoryManager handling pickup: {item.ItemName}, Quantity: {quantity}");

        //add inventory limit maxSlots if statement return

        //check if item already exists
        InventorySlot existingSlot = inventorySlots.Find(slot => slot.Item == item);    //lambda expression, returns null if no match
        //check if item is stackable, if the item already exists
        if (existingSlot != null && item.MaxStackSize > 1)
        {
            //store number of that item that can fit in the current stack
            int availableSpace = item.MaxStackSize - existingSlot.Quantity;
            //store how many of the items collected to be stored in the current stack
            //i.e. if max stack = 10, player has 7 and collects 5, only 3 will be stored
            int addedQuantity = Mathf.Min(availableSpace, quantity);
            //adjust stack number
            existingSlot.Quantity += addedQuantity;

            //Debug.Log($"Existing Slot had {quantity} of {item.ItemName} added to it with a total of {existingSlot.Quantity} in the slot.");

            //adjust quantity for possible left overs
            quantity -= addedQuantity;
        }

        //adjust weapon ammo if already in inventory
        if (existingSlot != null && item.ItemType_ == ItemBase.ItemType.Weapon)
        {
            //add the ammo only
            WeaponInformation weapon = (WeaponInformation)existingSlot.Item;

            weapon.ammoStored += weapon.maxClipAmmo;
        }
        else if(quantity > 0)                                                   //new slot if no stack or if left over and not weapon type
        {
            inventorySlots.Add(new InventorySlot(item, quantity));

            //Debug.Log($"New Slot created with item {item.ItemName} and quantity {quantity}");
        }

        //Debug.Log($"Added {quantity} of {item.ItemName} to inventory.");

        if (item.ItemType_ == ItemBase.ItemType.Weapon)
        {
            WeaponInAction weaponsToUpdate = player.GetComponent<WeaponInAction>();

            if (weaponsToUpdate != null)
            {
                weaponsToUpdate.CheckAvailableWeapons();
            }
        }

        //notifies that inventory was updated
        OnInventoryUpdated?.Invoke();   //Unity event (for other managers to listen for)
    }

    public void OnDrop(ItemBase item, int quantity)
    {
        //find the item slot
        InventorySlot slot = inventorySlots.Find(s => s.Item == item);  //lambda expression

        if (slot != null)
        {
            //logic for stackable items
            if (item.MaxStackSize > 1)
            {
                //adjust stack quantity
                slot.Quantity -= quantity;

                //remove slot if no more items in it
                if (slot.Quantity <= 0)
                {
                    inventorySlots.Remove(slot);
                }
            }
            else
                inventorySlots.Remove(slot);    //go straight to removing slot if not stackable

            //notify other systems that the inventory has been updated
            OnInventoryUpdated?.Invoke();   //Unity event (for other managers to listen for)
        }
        //else
            //Debug.Log($"Item: {item.ItemName} not found in inventory");
    }

    //called to update UI (for possible future use)
    void UpdateUI()
    {
        //event to update UI
        //Debug.Log("Inventory UI updated.");
    }

    //=============================SAVE AND LOAD INVENTORY TO/FROM SAVE DATA=============================

    public void SaveInventoryData()
    {
        //get save data from scenemanager
        SaveData data = SceneManagerScript.instance.SaveData;

        //clear old data
        data.inventorySlots.Clear();

        //add each item in the inventory to be saved
        foreach (InventorySlot slot in inventorySlots)
        {
            InventorySlotData slotData = new InventorySlotData();
            slotData.itemName = slot.Item.ItemName;
            slotData.quantity = slot.Quantity;

            //store in the save data list
            data.inventorySlots.Add(slotData);
        }

        //save game to memory
        //SceneManagerScript.instance.SaveGame();
    }

    public void LoadInventoryData()
    {
        //get save data from scene manager
        SaveData data = SceneManagerScript.instance.SaveData;

        //clear any current inventory
        inventorySlots.Clear();

        //load each slot
        foreach(InventorySlotData slotData in data.inventorySlots)
        {
            ItemBase item = GetItemByName(slotData.itemName);
            if (item != null)
            {
                InventorySlot newSlot = new InventorySlot(item, slotData.quantity);
                inventorySlots.Add(newSlot);
            }
        }

        OnInventoryUpdated?.Invoke();
    }

    private ItemBase GetItemByName(string itemName)
    {
        foreach(ItemBase item in itemsLoad)
        {
            if(item.ItemName == itemName)
                return item;
        }

        return null;
    }

    //=============================SAVE AND LOAD INVENTORY TO/FROM SAVE DATA=============================

    
    //update important items to keep track off
    public void CollectEnergyCell()
    {
        if(SceneManagerScript.instance != null)
        {
            SceneManagerScript.instance.SaveData.energyCellsCollected++;
            SceneManagerScript.instance.SaveGame();                         //possibly not save when this happens?
        }
    }

    public void CollectShard()
    {
        if (SceneManagerScript.instance != null)
        {
            SceneManagerScript.instance.SaveData.shardsCollected++;
            SceneManagerScript.instance.SaveGame();                         //possibly not save when this happens?
        }
    }
}

public class InventorySlot
{
    //ItemBase = scriptable object for items (abstract class)
    public ItemBase Item { get; private set; }
    public int Quantity { get; set; }

    public InventorySlot(ItemBase item, int quantity)
    {
        Item = item;
        Quantity = quantity;
    }
}
