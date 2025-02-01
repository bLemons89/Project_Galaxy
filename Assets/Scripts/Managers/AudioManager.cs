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

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        source_2D = this.GetComponent<AudioSource>();
        source_Player = GameObject.Find("Player_Skinned Variant").GetComponent<AudioSource>();

        //if (sfxMaster != null) source_Player.outputAudioMixerGroup = sfxMaster;

        //source_2D.loop = true;
        PlayMusic(MenuMusic[0]);
    }


    //public void PlayMusic(AudioSource source, AudioClip[] arrayName, string clipName = "")
    public void PlayMusic(AudioClip music)
    {
        source_2D.clip = music;
        source_2D.loop = true;
        source_2D.Play();

        //AudioClip audioClip = null;

        //// if clip name is passed
        //if (!string.IsNullOrEmpty(clipName))
        //{
        //    audioClip = Array.Find(arrayName, clip => clip.name == clipName);
        //}
        //else
        //{
        //    // If no clip name is provided, pick a random sound
        //    audioClip = arrayName[UnityEngine.Random.Range(0, arrayName.Length)];
        //}
        //if (audioClip == null) return;
    }

    //public void PlaySFX(AudioClip[] arrayName, string clipName = "")
    public void PlaySFX(AudioClip clipSFX)
    {
        source_Player.clip = clipSFX;
        source_Player.PlayOneShot(clipSFX);

        //   AudioClip audioClip = null;

        //// if clip name is passed
        //if (!string.IsNullOrEmpty(clipName))
        //    {
        //        audioClip = Array.Find(arrayName, clip => clip.name == clipName);
        //    }
        //    else
        //    {
        //    audioClip = arrayName[UnityEngine.Random.Range(0, arrayName.Length)];
        //    }

        //    if (audioClip == null)
        //    {
        //        return;
        //    }
        // play the sound on the player audio source
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