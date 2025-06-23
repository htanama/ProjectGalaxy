using UnityEditor;
using UnityEngine;

/*
    To expose the audio settings and fields in the editor
    for mixing and saving.
    Keeps the class fields from getting to long.
    This file MUST be in the Editor folder
 */

[CustomEditor(typeof(AudioSettings))]
public class AudioSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AudioSettings audioSettings = (AudioSettings)target;

        // For each audio array, display a slider for volume and a toggle for mute
        EditorGUILayout.Space();
        GUILayout.Label("Master Controls", EditorStyles.boldLabel);
        DrawSettings("Master Music",    ref audioSettings.musicMaster,  ref audioSettings.musicMasterMute);
        DrawSettings("Master SFX",      ref audioSettings.sfxMaster,    ref audioSettings.sfxMasterMute);

        EditorGUILayout.Space();
        GUILayout.Label("Music Categories", EditorStyles.boldLabel);
        DrawSettings("Game Music",      ref audioSettings.gameMusicVolume, ref audioSettings.gameMusicMute);
        DrawSettings("Menu Music",      ref audioSettings.menuMusicVolume, ref audioSettings.menuMusicMute);
        DrawSettings("Level Music",     ref audioSettings.levelMusicVolume, ref audioSettings.levelMusicMute);
        DrawSettings("Ambient Music",   ref audioSettings.ambientNoiseVolume, ref audioSettings.ambientNoiseMute);

        EditorGUILayout.Space();
        GUILayout.Label("SFX Categories", EditorStyles.boldLabel);
        GUILayout.Label("Player", EditorStyles.label);
        DrawSettings("Master Player",   ref audioSettings.masterPlayerVolume, ref audioSettings.masterPlayerMute);
        DrawSettings("Footsteps",       ref audioSettings.playerStepVolume, ref audioSettings.playerStepMute);
        DrawSettings("Jump",            ref audioSettings.playerJumpVolume, ref audioSettings.playerJumpMute);
        DrawSettings("Land",            ref audioSettings.playerLandVolume, ref audioSettings.playerLandMute);
        DrawSettings("Hurt",            ref audioSettings.playerHurtVolume, ref audioSettings.playerHurtMute);

        EditorGUILayout.Space();
        GUILayout.Label("Weapons", EditorStyles.label);
        DrawSettings("Master Weapon",   ref audioSettings.masterWeaponVolume, ref audioSettings.masterWeaponMute);
        DrawSettings("Bullet Shoot",    ref audioSettings.bulletShootVolume, ref audioSettings.bulletShootMute);
        DrawSettings("Laser Shoot",     ref audioSettings.laserShootVolume, ref audioSettings.laserShootMute);

        EditorGUILayout.Space();
        GUILayout.Label("UI", EditorStyles.label); 
        DrawSettings("Master UI",       ref audioSettings.masterUIVolume, ref audioSettings.masterUIMute);
        DrawSettings("Button Sounds",   ref audioSettings.buttonSoundsVolume, ref audioSettings.buttonSoundsMute);
        DrawSettings("Warning",         ref audioSettings.warningVolume, ref audioSettings.warningMute);
        DrawSettings("Approve",         ref audioSettings.approveVolume, ref audioSettings.approveMute);
        DrawSettings("Deny",            ref audioSettings.denyVolume, ref audioSettings.denyMute);
        DrawSettings("Open",            ref audioSettings.openVolume, ref audioSettings.openMute);
        DrawSettings("Close",           ref audioSettings.closeVolume, ref audioSettings.closeMute);

        EditorGUILayout.Space();
        GUILayout.Label("Environment", EditorStyles.label);
        DrawSettings("Master Environment", ref audioSettings.masterEnvironmentVolume, ref audioSettings.masterEnvironmentMute);
        DrawSettings("Objects",         ref audioSettings.objectsVolume, ref audioSettings.objectsMute);
        DrawSettings("Pickup",          ref audioSettings.pickupVolume, ref audioSettings.pickupMute);

        EditorGUILayout.Space();
        GUILayout.Label("Enemy", EditorStyles.label);
        DrawSettings("Master Enemy",    ref audioSettings.masterEnemyVolume, ref audioSettings.masterEnemyMute);
        DrawSettings("Enemy Footsteps", ref audioSettings.enemyStepsVolume, ref audioSettings.enemyStepsMute);
        DrawSettings("Enemy Hurt",      ref audioSettings.enemyHurtVolume, ref audioSettings.enemyHurtMute);
        DrawSettings("Enemy Die",       ref audioSettings.enemyDieVolume, ref audioSettings.enemyDieMute);

        // Mark the object as dirty so Unity saves it
        if (GUI.changed)
        {
            EditorUtility.SetDirty(audioSettings);
        }
    }

    private void DrawSettings(string label, ref float volume, ref bool mute)
    {
        volume = EditorGUILayout.Slider(label + " Volume", volume, 0f, 1f);
        mute = EditorGUILayout.Toggle(label + " Mute", mute);
    }
};



