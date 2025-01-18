using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

#region Static Instance

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    public static AudioManager Instance
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

        musicSource = this.gameObject.AddComponent<AudioSource>();
        musicSource2 = this.gameObject.AddComponent<AudioSource>();
        sfxSource = this.gameObject.AddComponent<AudioSource>();

        // Music Sorce loops, just keep them going
        musicSource.loop = true;
        musicSource2.loop = true;
    }

    public void PlayMusic(AudioClip musicClip)
    {
        //Determine which source is playing
        AudioSource activeSource = (IsPlaying) ? musicSource : musicSource2;

        activeSource.clip = musicClip;
        activeSource.volume = 1;
        activeSource.Play();
    }

    public void PlayMusicwithFade(AudioClip newClip, float transitionTime = 1.0f)
    {
        AudioSource activeSource = (IsPlaying) ? musicSource : musicSource2;

        StartCoroutine(UpdateMusicWithFade(activeSource, newClip, transitionTime));
    }

    public void PlayMusicWithCrossFade(AudioClip musicClip, float transitionTime = 1.0f)
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

    private IEnumerator UpdateMusicWithFade(AudioSource activeSource, AudioClip newClip, float transitionTime)
    {
        // Make Sure the source is active and playing
        if (!activeSource.isPlaying)
        {
            activeSource.Play();

            float t = 0.0f;

            //fade out music
            for (t = 0.0f; t < transitionTime; t += Time.deltaTime)
            {
                activeSource.volume = (1 - (t / transitionTime));
                yield return null;
            }
            activeSource.Stop();
            activeSource.clip = newClip;
            activeSource.Play();

            // fade in music
            for(t = 0.0f; t < transitionTime; t += Time.deltaTime)
            {
                activeSource.volume = (t / transitionTime);
                yield return null;
            }
        }
    }

    private IEnumerator UpdateMusicWithCrossFade(AudioSource original, AudioSource newSource, float transitionTime)
    {
        float t = 0.0f;

        for(t = 0.0f; t < transitionTime; t += Time.deltaTime)
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
