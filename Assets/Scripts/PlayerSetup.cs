using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    [SerializeField] DataScriptableObject dataForThisLevel;
    [SerializeField] CharacterController playerController;

    // Start is called before the first frame update
    void Start()
    {
        if (playerController != null)
        {
            playerController.transform.position = dataForThisLevel.spawnLocationVec3;
        }
    }

 
}
