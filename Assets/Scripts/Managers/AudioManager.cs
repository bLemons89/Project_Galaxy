using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;    

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, playerSFX;

    private void Awake()
    {
            instance = this;       
    }
    private void Start()
    {
        PlayMusic("LostSignal");
    }
    public void PlayMusic(string name)
    {
        Sound sound = Array.Find(musicSounds, targetSound => targetSound.name == name);

        if (sound == null || sound.clips == null || sound.clips.Length == 0)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            //choose a clip
            AudioClip clipToPlay = sound.clips[UnityEngine.Random.Range(0, sound.clips.Length)];
            musicSource.clip = clipToPlay;
            musicSource.Play();

        }
    }

    public void PlaySFX(string name, AudioSource sfxSource)
    {
        Sound sound = Array.Find(sfxSounds, targetSound => targetSound.name == name);

        if (sound == null || sound.clips == null || sound.clips.Length == 0)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            //sfxSource.PlayOneShot(sound.clip);
            //choose a clip
            AudioClip clipToPlay = sound.clips[UnityEngine.Random.Range(0, sound.clips.Length)];
            sfxSource.PlayOneShot(clipToPlay);
            //sfxSource.Play();
        }
    }

    // Toggle //
    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }
    public void MuteAllSFX()
    {
        playerSFX.mute = true;
    }
    public void UnMuteAllSFX()
    {
        playerSFX.mute = false;
    }
    public void TogglePlayerSFX()
    {
        playerSFX.mute = !playerSFX.mute;
    }



    // Volume //
    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    public void SFXAllVolume(float volume)
    {
        playerSFX.volume = volume;
        //add others
    }
    public void SFXPlayerVolume(float volume)
    {
        playerSFX.volume = volume;
    }
}
