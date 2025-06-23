using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

// Put this script on the PlayerEntrance to spawn the player into the location here

public class PlayerExit : MonoBehaviour
{
    public static PlayerExit instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // In the begining of the game we need to put the player in the center of the ship   
        
        // else player coming from outside the ship, will use the PlayerEntrance object transform position
        CharacterController playerEntranceLocation = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
                
        // assigning the PlayerEntrance object position to the Player.transform.position
        playerEntranceLocation.transform.position = transform.position;
        
    }

}
