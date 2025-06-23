/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/26/2025
    Date Updated: 02/01/2025
    Description: Class to simply toggle text on and off
 */
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToggleText : MonoBehaviour
{
    [SerializeField] GameObject textToToggle;

    bool playerInRange;

    private void Update()
    {
        if (playerInRange)
        {
            textToToggle.SetActive(true);
        }
        else
            textToToggle.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))                       //ensure it's the player entering the trigger
        {
            playerInRange = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))                       //ensure it's the player leaving the trigger
        {
            playerInRange = false;
        }
    }
}
