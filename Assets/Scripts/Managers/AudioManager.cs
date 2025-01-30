using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("===== Audio Mixers =====")]
    [SerializeField] public AudioMixer audioMixer;
    [SerializeField] public AudioMixerGroup sfxMaster;
    //[SerializeField] public AudioSettings audioSettings;
    //[SerializeField] public MixerAdapter mixerAdapter;

    [Header("===== Audio Sources =====")]
    public AudioSource source_2D;
    public AudioSource source_Player;

    [Header("===== Audio Arrays =====")]
    public Sound[] GameMusic;
    public Sound[] MenuMusic;
    public Sound[] LevelMusic;
    public Sound[] AmbientNoise;
    public Sound[] PlayerSounds;
    public Sound[] Weapons;
    public Sound[] UI;
    public Sound[] Environment;
    public Sound[] Enemy;

    private void Awake()
    {
        instance = this;
    }       
    private void Start()
    {
        source_2D = GameManager.instance.GetComponent<AudioSource>();
        source_Player = GameObject.FindWithTag("Player").GetComponent<AudioSource>();
        AudioManager.instance.GetComponent<AudioMixer>();
        //AudioManager.instance.GetComponent<MixerAdapter>();

        if (sfxMaster != null) source_Player.outputAudioMixerGroup = sfxMaster;

        source_2D.loop = true;
        PlayMusic(source_2D, MenuMusic,"LostSignal");
    }
    
    
    public void PlayMusic(AudioSource source, Sound[] arrayName, string clipName = "")
    {
        Sound sound = null;

        // if clip name is passed
        if (!string.IsNullOrEmpty(clipName))
        {
            sound = Array.Find(arrayName, targetSound => targetSound.name == clipName);
        }
        else
        {
            // If no clip name is provided, pick a random sound
            sound = arrayName[UnityEngine.Random.Range(0, arrayName.Length)];
        }
        
        if (sound == null || sound.clips == null || sound.clips.Length == 0)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            AudioClip clipToPlay = sound.clips[0];
            source.clip = clipToPlay;
            source.Play();
        }
    }

    public void PlaySFX(Sound[] arrayName, string clipName = "")
    {
        Sound sound = null;

        // if clip name is passed
        if (!string.IsNullOrEmpty(clipName))
        {
            sound = Array.Find(arrayName, targetSound => targetSound.name == clipName);
        }
        else
        {
            sound = arrayName[UnityEngine.Random.Range(0, arrayName.Length)];
        }

        if (sound == null || sound.clips == null || sound.clips.Length == 0)
        {
            Debug.Log("Sound Not Found");
            return;
        }
            AudioClip clipToPlay = sound.clips[0];
        
        // play the sound on the player audio source
        source_Player.PlayOneShot(clipToPlay);
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
