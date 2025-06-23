using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DataScriptableObject : ScriptableObject
{
    [Header("Scene Location")]
    public string currentSceneName;
    public string loadingSceneName;
    public string nextSceneName;

    [Header("Player Data")]
    public string prefabName;
    public float HealthPoint;
    public Vector3 spawnLocationVec3;

    [Header("Access to GameObject If Needed")]
    public GameObject gameObjct;


    [Header("Music and Audio Data")]
    public AudioManager audioManager;
}
