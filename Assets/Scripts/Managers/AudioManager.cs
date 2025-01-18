using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Playables;

#region Static Instance

public class AudioManager : MonoBehaviour
{
    private AudioManager instance;

    public AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if (instance == null)
                {
                    instance = new GameObject("Spawned AudioManager", typeof(AudioManager)).GetComponent<AudioManager>();
                }
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    #endregion

    #region Fields

    private AudioSource musicSource;
    private AudioSource musicSource2;
    private AudioSource sfxSource;

    private bool IsPlaying;

    // there are two sources to crossfade easily

    #endregion

    private void Awake()
    {
        // make sure we don't destroy this instance
        DontDestroyOnLoad(this.gameObject);

        musicSource = GetComponent<AudioSource>();
        musicSource2 = GetComponent<AudioSource>();
        sfxSource = GetComponent<AudioSource>();

        // Music Sorce loops, just keep them going
        musicSource.loop = true;
        musicSource2.loop = true;
    }

    public void PlayMusic(AudioClip musicClip)
    {
        //Determin which source is playing
        AudioSource activeSource = (IsPlaying) ? musicSource : musicSource2;
        musicSource.clip = musicClip;
        musicSource.Play();
    }

    public void PlayMusicWithFade(AudioClip musicClip, float transitionTime = 1.0f)
    {
        AudioSource activeSource = (IsPlaying) ? musicSource : musicSource2;
        AudioSource newSource = (IsPlaying) ? musicSource2 : musicSource;

        // Swap source audio playing
        IsPlaying = !IsPlaying;

        // set the fields of the audio source, then start the coroutine
        newSource.clip = musicClip;
        newSource.Play();
        StartCoroutine(UpdateMusicWithCrossFade(activeSource, newSource, transitionTime));
    }

    IEnumerator UpdateMusicWithFade(AudioSource activeSource, AudioClip newClip, float transitionTime)
    {
        // Make Sure the source is active and playing
        if (!activeSource.isPlaying)
        {
            activeSource.Play();

            float t = 0.0f;

            //fade out music
            for (t = 0.0f; t <= transitionTime; t += Time.deltaTime)
            {
                activeSource.volume = (1 - (t / transitionTime));
                yield return null;
            }
            activeSource.Stop();
            activeSource.clip = newClip;
            activeSource.Play();

            // fad in music
            for(t = 0.0f; t <= transitionTime; t+= Time.deltaTime)
            {
                activeSource.volume += (1 - (t / transitionTime));
                yield return null;
            }
        }
    }

    private IEnumerator UpdateMusicWithCrossFade(AudioSource original, AudioSource newSource, float transitionTime)
    {
        float t = 0.0f;

        for(t = 0.0f; t<= transitionTime; t += Time.deltaTime)
        {
            original.volume = (1 - (t / transitionTime));
            newSource.volume = (t / transitionTime);
            yield return null;
        }
        original.Stop();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlaySFX(AudioClip clip, float volume)
    {
        sfxSource.PlayOneShot(clip, volume);
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        musicSource2.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
