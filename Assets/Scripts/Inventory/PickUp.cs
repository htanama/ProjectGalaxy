/*
    Author: Breanna Lemons
    Edited By: Juan Contreras
    Date Created: 01/16/2025
    Date Updated: 01/31/2025
    Description: Class to use when picking up objects in a scene to be
                 stored in the inventory.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickUp : MonoBehaviour
{
    [Header("Scriptable Here")]
    [SerializeField] ItemBase item;
    [SerializeField] int quantity = 1;  //number of that item this pickup will add to the inventory
    [SerializeField] GameObject textToToggle;

    //for saving/loading
    UniqueID uniqueID;

    bool playerInRange;
    //public UnityEvent<ItemBase, int> OnPickup;
    //public UnityEvent OnWeaponPickup;

    private void Start()
    {
        uniqueID = GetComponent<UniqueID>();

        //if (SceneManagerScript.instance.SaveData.destroyedObjects.Contains(uniqueID.ID))
        //{ Destroy(gameObject); }        //destroyed if picked up in past save
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Q))     //check if the player presses F
        {
            //try to drop off the collectible item
            PickUpItem();
        }
    }

    void OnTriggerEnter(Collider other)         //add key press input check in if statement
    {
        //prevent other triggers from triggering??

        //check for player collider
        if(other.CompareTag("Player"))
        {
            if (textToToggle != null)
                textToToggle.SetActive(true);

            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //check for player collider
        if (other.CompareTag("Player"))
        {
            if (textToToggle != null)
                textToToggle.SetActive(false);

            playerInRange = false;
        }
    }

    void PickUpItem()
    {
        if (item != null && InventoryManager.instance != null)
        {
            InventoryManager.instance.OnPickup(item, quantity);

            //mark as destroyed for saving
            SceneManagerScript.instance.MarkObjectAsDestroyed(uniqueID.ID);
            SceneManagerScript.instance.SaveGame();

            //destroy item in the world
            Destroy(gameObject);
            //AudioManager.instance.PlaySFX(AudioManager.instance.AR_Sounds[2]);
        }
    }    

}
