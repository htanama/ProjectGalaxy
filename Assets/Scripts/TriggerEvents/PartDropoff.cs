/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/25/2025
    Date Updated: 01/26/2025
    Description: Class to handle the text visuals when player drops off an item
 */
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartDropoff : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] GameObject textObjectToToggle;        //text element to display message
    [SerializeField] TextMeshPro partsCountText;     //text element to display the number of parts inserted

    [Header("References")]
    [SerializeField] InventoryManager playerInventory;    //reference to the player's InventoryManager
    [SerializeField] GameObject[] meshesToToggle;

    int insertedParts = 0;              //tracks the number of parts inserted
    bool playerInRange = false;         //checks if the player is within the trigger

    void Start()
    {
        UpdatePartsText();
        UpdatePartsMesh();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))                       //ensure it's the player entering the trigger
        {
            playerInRange = true;
            textObjectToToggle.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))                       //ensure it's the player leaving the trigger
        {
            playerInRange = false;
            textObjectToToggle.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))     //check if the player presses F
        {
            //try to drop off the collectible item
            DropOffCollectible();
            UpdatePartsMesh();
        }

        if(insertedParts >= 3)
        {
            Time.timeScale = 0;
            GameManager.instance.GetComponent<ButtonFunctions>().WinScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void DropOffCollectible()
    {
        //check the player's inventory for a collectible item (ship part)
        InventorySlot missionItemSlot = playerInventory.InventorySlotsList.Find(slot => slot.Item.ItemType_ == ItemBase.ItemType.MissionItem);

        if (missionItemSlot != null)
        {
            //remove one collectible item from the inventory
            missionItemSlot.Quantity--;
            insertedParts++;

            //if the slot is empty, remove it from the inventory
            if (missionItemSlot.Quantity <= 0)
            {
                playerInventory.InventorySlotsList.Remove(missionItemSlot);
            }

            //update the UI
            UpdatePartsText();
            Debug.Log("Dropped off a collectible item");
        }
        else
        {
            Debug.Log("No collectible items in the player's inventory");
        }
    }

    void UpdatePartsText()
    {
        //update the text showing the number of parts inserted
        partsCountText.text = $"{insertedParts}";
    }

    void UpdatePartsMesh()
    {
        //enable or disable objects based on the number of parts inserted
        for (int i = 0; i < meshesToToggle.Length; i++)
        {
            //enable objects progressively based on inserted parts
            meshesToToggle[i].SetActive(i < insertedParts);
        }
    }
}
