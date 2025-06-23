using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Door_Open_Close : MonoBehaviour
{
    public Animator myAnimation;

    private void Start()
    {
        myAnimation = GetComponent<Animator>();
    }

    // On Enter trigger to open door
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            myAnimation.SetTrigger("Open");
        }
    }

    // On Exit trigger to close door
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            myAnimation.SetTrigger("Close"); ;
        }
    }
}
