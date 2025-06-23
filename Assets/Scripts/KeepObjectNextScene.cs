using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepObjectNextScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        Debug.Log($"Do NOT destory this object on next scene {gameObject.scene.name}");
    }
  
}
