using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.DesignPatterns;
public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioSource musicAudioSource;

    [SerializeField] List<AudioClip> combatBGMs;

    // Play a sound effect
    public void PlaySFX(AudioClip clip)
    {
        sfxAudioSource.PlayOneShot(clip);
    }

    // Play background music
    public void PlayMusic(AudioClip clip)
    {
        musicAudioSource.clip = clip;
        musicAudioSource.Play();
    }

    // Stop playing background music
    public void StopMusic()
    {
        musicAudioSource.Stop();
    }

    // Set the volume of sound effects
    public void SetSFXVolume(float volume)
    {
        sfxAudioSource.volume = volume;
    }

    // Set the volume of background music
    public void SetMusicVolume(float volume)
    {
        musicAudioSource.volume = volume;
    }
}
