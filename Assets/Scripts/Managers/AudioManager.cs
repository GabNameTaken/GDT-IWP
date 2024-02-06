using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.DesignPatterns;
public class AudioManager : SingletonPersistent<AudioManager>
{
    public AudioSource sfxAudioSource;
    public AudioSource musicAudioSource;

    private const string SFXVolumeKey = "SFXVolume";
    private const string MusicVolumeKey = "MusicVolume";

    [SerializeField] AudioClip mainMenuBGM;
    [SerializeField] AudioClip[] combatBGMs;

    [Header("Common audio")]
    [SerializeField] AudioClip healSFX;

    private void Start()
    {
        // Load the saved volumes
        float savedSFXVolume = PlayerPrefs.GetFloat(SFXVolumeKey, 1.0f);
        float savedMusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1.0f);

        SetSFXVolume(savedSFXVolume);

        SetMusicVolume(savedMusicVolume);
    }

    // Play a sound effect
    public void PlaySFX(AudioClip clip)
    {
        sfxAudioSource.PlayOneShot(clip);
    }

    // Play background music
    public void PlayMusic(AudioClip clip, bool loop)
    {
        musicAudioSource.clip = clip;
        musicAudioSource.loop = loop;
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
        PlayerPrefs.SetFloat(SFXVolumeKey, volume);
    }

    // Set the volume of background music
    public void SetMusicVolume(float volume)
    {
        musicAudioSource.volume = volume;
        PlayerPrefs.SetFloat(MusicVolumeKey, volume);
    }

    public void PlayMainMenuBGM()
    {
        PlayMusic(mainMenuBGM, true);
    }

    public void PlayRandomCombatBGM()
    {
        if (combatBGMs.Length == 0)
        {
            Debug.LogWarning("No music tracks assigned.");
            return;
        }

        int randomIndex = Random.Range(0, combatBGMs.Length);
        AudioClip randomMusic = combatBGMs[randomIndex];

        PlayMusic(randomMusic, true);
    }

    public void PlayHealSFX()
    {
        PlaySFX(healSFX);
    }
}
