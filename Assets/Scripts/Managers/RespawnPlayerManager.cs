using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayerManager : MonoBehaviour
{
    // use the tag
    public GameObject playerObject;

    void Start()
    {
        // Respawn the player where it last location
        GameObject.Find("Area1Exit").SetActive(false);
    }
}
