//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;
//using UnityEngine.Audio;

//public class MixerAdapter : MonoBehaviour
//{
//    public Dictionary<string, (float volume, bool mute)> AudioVolumeSettings;

//    private void Start()
//    {
//        InitializeDictionary();
//    }
//    public void InitializeDictionary()
//    {
//        AudioVolumeSettings = new Dictionary<string, (float volume, bool mute)>(); 
//    }
//    public void LoadDictionaryData() 
//    {
//        if (AudioVolumeSettings == null)
//        {
//            InitializeDictionary();

//            AudioVolumeSettings["musicMaster"]      = (1f, false);
//            AudioVolumeSettings["sfxMaster"]        = (1f, false);

//            AudioVolumeSettings["gameMusic"]        = (1f, false);
//            AudioVolumeSettings["menuMusic"]        = (1f, false);
//            AudioVolumeSettings["levelMusic"]       = (1f, false);
//            AudioVolumeSettings["ambientNoise"]     = (1f, false);

//            AudioVolumeSettings["masterPlayer"]     = (1f, false);
//            AudioVolumeSettings["playerStep"]       = (1f, false);
//            AudioVolumeSettings["playerJump"]       = (1f, false);
//            AudioVolumeSettings["playerLand"]       = (1f, false);
//            AudioVolumeSettings["playerHurt"]       = (1f, false);

//            AudioVolumeSettings["masterWeapon"]     = (1f, false);
//            AudioVolumeSettings["bulletShoot"]      = (1f, false);
//            AudioVolumeSettings["laserShoot"]       = (1f, false);

//            AudioVolumeSettings["masterUI"]         = (1f, false);
//            AudioVolumeSettings["buttonSounds"]     = (1f, false);
//            AudioVolumeSettings["warning"]          = (1f, false);
//            AudioVolumeSettings["approve"]          = (1f, false);
//            AudioVolumeSettings["deny"]             = (1f, false);
//            AudioVolumeSettings["open"]             = (1f, false);
//            AudioVolumeSettings["close"]            = (1f, false);

//            AudioVolumeSettings["masterEnvironment"] = (1f, false);
//            AudioVolumeSettings["objects"]          = (1f, false);
//            AudioVolumeSettings["pickup"]           = (1f, false);

//            AudioVolumeSettings["masterEnemy"]      = (1f, false);
//            AudioVolumeSettings["enemySteps"]       = (1f, false);
//            AudioVolumeSettings["enemyHurt"]        = (1f, false);
//            AudioVolumeSettings["enemyDie"]         = (1f, false);
//        }
//        else   
//        {
//            UpdateDictionary();
//        }
//    }
//    public void UpdateDictionary()
//    {
//        AudioVolumeSettings["musicMaster"]  = (AudioManager.instance.audioSettings.musicMaster, AudioManager.instance.audioSettings.musicMasterMute);
//        AudioVolumeSettings["sfxMaster"]    = (AudioManager.instance.audioSettings.sfxMaster, AudioManager.instance.audioSettings.sfxMasterMute);

//        AudioVolumeSettings["gameMusic"]    = (AudioManager.instance.audioSettings.gameMusicVolume, AudioManager.instance.audioSettings.gameMusicMute);
//        AudioVolumeSettings["menuMusic"]    = (AudioManager.instance.audioSettings.menuMusicVolume, AudioManager.instance.audioSettings.menuMusicMute);
//        AudioVolumeSettings["levelMusic"]   = (AudioManager.instance.audioSettings.levelMusicVolume, AudioManager.instance.audioSettings.levelMusicMute);
//        AudioVolumeSettings["ambientNoise"] = (AudioManager.instance.audioSettings.ambientNoiseVolume, AudioManager.instance.audioSettings.ambientNoiseMute);

//        AudioVolumeSettings["masterPlayer"] = (AudioManager.instance.audioSettings.masterPlayerVolume, AudioManager.instance.audioSettings.masterPlayerMute);
//        AudioVolumeSettings["playerStep"]   = (AudioManager.instance.audioSettings.playerStepVolume, AudioManager.instance.audioSettings.playerStepMute);
//        AudioVolumeSettings["playerJump"]   = (AudioManager.instance.audioSettings.playerJumpVolume, AudioManager.instance.audioSettings.playerJumpMute);
//        AudioVolumeSettings["playerLand"]   = (AudioManager.instance.audioSettings.playerLandVolume, AudioManager.instance.audioSettings.playerLandMute);
//        AudioVolumeSettings["playerHurt"]   = (AudioManager.instance.audioSettings.playerHurtVolume, AudioManager.instance.audioSettings.playerHurtMute);

//        AudioVolumeSettings["masterWeapon"] = (AudioManager.instance.audioSettings.masterWeaponVolume, AudioManager.instance.audioSettings.masterWeaponMute);
//        AudioVolumeSettings["bulletShoot"]  = (AudioManager.instance.audioSettings.bulletShootVolume, AudioManager.instance.audioSettings.bulletShootMute);
//        AudioVolumeSettings["laserShoot"]   = (AudioManager.instance.audioSettings.laserShootVolume, AudioManager.instance.audioSettings.laserShootMute);

//        AudioVolumeSettings["masterUI"]     = (AudioManager.instance.audioSettings.masterUIVolume, AudioManager.instance.audioSettings.masterUIMute);
//        AudioVolumeSettings["buttonSounds"] = (AudioManager.instance.audioSettings.buttonSoundsVolume, AudioManager.instance.audioSettings.buttonSoundsMute);
//        AudioVolumeSettings["warning"]      = (AudioManager.instance.audioSettings.warningVolume, AudioManager.instance.audioSettings.warningMute);
//        AudioVolumeSettings["approve"]      = (AudioManager.instance.audioSettings.approveVolume, AudioManager.instance.audioSettings.approveMute);
//        AudioVolumeSettings["deny"]         = (AudioManager.instance.audioSettings.denyVolume, AudioManager.instance.audioSettings.denyMute);
//        AudioVolumeSettings["open"]         = (AudioManager.instance.audioSettings.openVolume, AudioManager.instance.audioSettings.openMute);
//        AudioVolumeSettings["close"]        = (AudioManager.instance.audioSettings.closeVolume, AudioManager.instance.audioSettings.closeMute);

//        AudioVolumeSettings["masterEnvironment"] = (AudioManager.instance.audioSettings.masterEnvironmentVolume, AudioManager.instance.audioSettings.masterEnvironmentMute);
//        AudioVolumeSettings["objects"]      = (AudioManager.instance.audioSettings.objectsVolume, AudioManager.instance.audioSettings.objectsMute);
//        AudioVolumeSettings["pickup"]       = (AudioManager.instance.audioSettings.pickupVolume, AudioManager.instance.audioSettings.pickupMute);

//        AudioVolumeSettings["masterEnemy"]  = (AudioManager.instance.audioSettings.masterEnemyVolume, AudioManager.instance.audioSettings.masterEnemyMute);
//        AudioVolumeSettings["enemySteps"]   = (AudioManager.instance.audioSettings.enemyStepsVolume, AudioManager.instance.audioSettings.enemyStepsMute);
//        AudioVolumeSettings["enemyHurt"]    = (AudioManager.instance.audioSettings.enemyHurtVolume, AudioManager.instance.audioSettings.enemyHurtMute);
//        AudioVolumeSettings["enemyDie"]     = (AudioManager.instance.audioSettings.enemyDieVolume, AudioManager.instance.audioSettings.enemyDieMute);
//    }

//    public void ApplySettingsToMixer(Dictionary<string, (float volume, bool mute)> settings)
//    {
//        foreach (var setting in settings)
//        {
//            // For each sound category apply to AudioMixer
//            string key = setting.Key;
//            float volume = setting.Value.volume;
//            bool mute = setting.Value.mute;

//            string volumeParam = key + "Volume";
//            string muteParam = key + "Mute";

//            if(mute)
//            {
//                // Set the volume to 0 if muted
//                AudioManager.instance.audioMixer.SetFloat(volumeParam, -80f);
//            }
//            else
//            {
//                // Apply the volume value normally if not muted
//                AudioManager.instance.audioMixer.SetFloat(volumeParam, volume);
//            }
//        }
//    }
//}
