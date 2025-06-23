using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// how to save data in Unity using System.Serializable
// https://dev.to/arkilis/systemserializable-in-unity-25hm
[System.Serializable]
public class PlayerData
{   
    public float x;
    public float y;
    public float z;
    public float playTimer;
    public float CurrentHealth;

    public PlayerData(Vector3 position)
    {
        x = position.x;
        y = position.y;
        z = position.z;
    }

    
}
