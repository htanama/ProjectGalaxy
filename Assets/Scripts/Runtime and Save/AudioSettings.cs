using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
    Settings intended to make both mixing, saving and changing 
    audio easier.
    If you add an array, include the name of the array, a volume and a mute
    Sound[] arrayName, float volume, bool mute
    I separated the arrays from the controls so you can mass edit (hold alt and drag)
    and view the info more easily
 */
[CreateAssetMenu(fileName = "AudioSettings", menuName = "Audio/Settings", order = 1)]
public class AudioSettings : ScriptableObject
{
    [Range(0f, 1f)] public float musicMaster;
    [Range(0f, 1f)] public float sfxMaster;
    public bool musicMasterMute;
    public bool sfxMasterMute;

    [Range(0f, 1f)] public float gameMusicVolume;
    [Range(0f, 1f)] public float menuMusicVolume;     
    [Range(0f, 1f)] public float levelMusicVolume;    
    [Range(0f, 1f)] public float ambientNoiseVolume;
    public bool gameMusicMute;
    public bool menuMusicMute;
    public bool levelMusicMute;
    public bool ambientNoiseMute;

    [Range(0f, 1f)] public float masterPlayerVolume;
    [Range(0f, 1f)] public float playerStepVolume;    
    [Range(0f, 1f)] public float playerJumpVolume;                  
    [Range(0f, 1f)] public float playerLandVolume;
    [Range(0f, 1f)] public float playerHurtVolume;
    public bool masterPlayerMute;
    public bool playerStepMute;
    public bool playerJumpMute;
    public bool playerLandMute;
    public bool playerHurtMute;

    [Range(0f, 1f)] public float masterWeaponVolume;
    [Range(0f, 1f)] public float bulletShootVolume;
    [Range(0f, 1f)] public float laserShootVolume;
    public bool masterWeaponMute;
    public bool bulletShootMute;
    public bool laserShootMute;

    [Range(0f, 1f)] public float masterUIVolume;
    [Range(0f, 1f)] public float buttonSoundsVolume;                    
    [Range(0f, 1f)] public float warningVolume;                    
    [Range(0f, 1f)] public float approveVolume;                    
    [Range(0f, 1f)] public float denyVolume;                    
    [Range(0f, 1f)] public float openVolume;                    
    [Range(0f, 1f)] public float closeVolume;
    public bool masterUIMute;
    public bool buttonSoundsMute;
    public bool warningMute;
    public bool approveMute;
    public bool denyMute;
    public bool openMute;
    public bool closeMute;

    [Range(0f, 1f)] public float masterEnvironmentVolume;
    [Range(0f, 1f)] public float objectsVolume;
    [Range(0f, 1f)] public float pickupVolume;
    public bool masterEnvironmentMute;
    public bool objectsMute;
    public bool pickupMute;

    [Range(0f, 1f)] public float masterEnemyVolume;
    [Range(0f, 1f)] public float enemyStepsVolume;
    [Range(0f, 1f)] public float enemyHurtVolume;
    [Range(0f, 1f)] public float enemyDieVolume;
    public bool masterEnemyMute;
    public bool enemyStepsMute;
    public bool enemyHurtMute; 
    public bool enemyDieMute;
}
