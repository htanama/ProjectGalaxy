/*
    Author: Juan Contreras
    Edited By:
    Date Created: 01/30/2025
    Date Updated: 01/30/2025
    Description: UniqueID system to keep items/enemies persistence between scenes
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]     //allow script to run in the Editor
public class UniqueID : MonoBehaviour
{
    [SerializeField] string uniqueID;

    public string ID => uniqueID;

    // Start is called before the first frame update
    void Awake()
    {
        if(string.IsNullOrEmpty(uniqueID))
        {
            GenerateID();
        }
    }

    private void Start()
    {
        if (SceneManagerScript.instance.SaveData.destroyedObjects.Contains(uniqueID))       //might throw error during editing, play test first
        {
            Destroy(gameObject);
        }
    }

    private void GenerateID()
    {
        uniqueID = Guid.NewGuid().ToString();
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }
}
