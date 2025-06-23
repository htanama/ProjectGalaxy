using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject inventorySlotPrefab;  // Prefab for UI slots
    public Transform[] slotPositions;       // Predefined slot positions (set these in the Inspector)

    private List<GameObject> slotInstances = new List<GameObject>();  // Tracks active UI slot instances

    private void OnEnable()
    {
        // Subscribe to Inventory Updates and refresh immediately when enabled
        if (InventoryManager.instance != null)
        {
            InventoryManager.instance.OnInventoryUpdated.AddListener(UpdateInventoryUI);
            UpdateInventoryUI();
        }
    }

    private void OnDisable()
    {
        if (InventoryManager.instance != null)
            InventoryManager.instance.OnInventoryUpdated.RemoveListener(UpdateInventoryUI);
    }

    public void UpdateInventoryUI()
    {
        // Clear previously instantiated UI slots
        foreach (GameObject slot in slotInstances)
        {
            Destroy(slot);
        }
        slotInstances.Clear();

        // Get the current inventory list
        List<InventorySlot> inventoryList = InventoryManager.instance.InventorySlotsList;

        // Limit to the number of available fixed slot positions
        int count = Mathf.Min(inventoryList.Count, slotPositions.Length);
        for (int i = 0; i < count; i++)
        {
            InventorySlot currentSlot = inventoryList[i];
            Transform slotPosition = slotPositions[i];

            // Instantiate the prefab as a child of the designated slot transform
            GameObject newSlot = Instantiate(inventorySlotPrefab, slotPosition);

            newSlot.SetActive(true);

            newSlot.transform.localPosition = Vector3.zero; // Align with the fixed position
            slotInstances.Add(newSlot);

            // Find UI components in the prefab
            Image icon = newSlot.transform.Find("ItemIcon").GetComponent<Image>();
            TMP_Text itemName = newSlot.transform.Find("ItemName").GetComponent<TMP_Text>();
            TMP_Text quantityText = newSlot.transform.Find("ItemAmount").GetComponent<TMP_Text>();
            TMP_Text maxQuantityText = newSlot.transform.Find("ItemMaxAmount").GetComponent<TMP_Text>();

            // Set the icon and item name
            icon.sprite = currentSlot.Item.Icon;
            itemName.text = currentSlot.Item.ItemName;

            // Adjust quantity display based on the item type
            switch (currentSlot.Item.ItemType_)
            {
                case ItemBase.ItemType.Weapon:
                    // For weapons, display the current ammo stored.
                    WeaponInformation weaponItem = currentSlot.Item as WeaponInformation;
                    if (weaponItem != null)
                    {
                        quantityText.text = weaponItem.currentAmmo.ToString();
                        maxQuantityText.text = "/ " + weaponItem.ammoStored;
                    }
                    else
                    {
                        quantityText.text = "";
                    }
                    break;

                default:
                    // For collectibles, mission items, ammo, and health items, display quantity
                    if (currentSlot.Item.MaxStackSize > 1)
                    {
                        quantityText.text = $"{currentSlot.Quantity}";
                        maxQuantityText.text = "/ " + currentSlot.Item.MaxStackSize;
                    }
                    else
                    {
                        quantityText.text = $"{currentSlot.Quantity}";
                    }
                    break;
            }
        }
    }
}
