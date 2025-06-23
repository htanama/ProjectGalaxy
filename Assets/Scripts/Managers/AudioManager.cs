using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("===== Audio Sources =====")]
    public AudioSource source_2D;
    public AudioSource source_Player;

    [Header("===== Audio Arrays =====")]
    public AudioClip[] GameMusic;
    public AudioClip[] MenuMusic;
    public AudioClip[] LevelMusic;

    [Header("===== Audio Player =====")]
    public AudioClip[] PlayerWalk;
    public AudioClip[] PlayerJump;
    public AudioClip[] PlayerDMG;

    [Header("===== Audio Weapons =====")]
    public AudioClip[] AR_Sounds;
    public AudioClip[] ER_Sounds;
    public AudioClip[] SH_Sounds;
    public AudioClip[] Empty_Clip;
    public AudioClip[] Reload;

    [Header(" ==== Audio UI ==== ")]
    public AudioClip[] UI_Menu;

    [Header(" ==== Audio Enemy ==== ")]
    public AudioClip[] EnemyDMG;
    public AudioClip[] EnemyDTH;

    bool firstBoot = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {

        if (source_Player == null)
        {
            source_2D = this.GetComponent<AudioSource>();
            source_Player = GameObject.FindWithTag("Player").GetComponent<AudioSource>();

            if (firstBoot)
            {
                PlayMusic(GameMusic[0]);

                firstBoot = false;
            }
        }
    }

    public void PlayMusic(AudioClip music)
    {
        source_2D.clip = music;
        source_2D.loop = true;
        source_2D.Play();
    }

    public void PlaySFX(AudioClip clipSFX)
    {
        source_Player.clip = clipSFX;
        source_Player.PlayOneShot(clipSFX);
    }

    // Toggle //
    //settings for mix and menu
    public void ToggleMusicSourceVol(AudioSource source)
    {
        source.mute = !source.mute;
    }

    // Volume //
    public void MusicVolume(AudioSource source, float volume)
    {
        source.volume = volume;
    }

    //Check for all implemented
    public void SFXAllVolume(float volume)
    {

    }

}