using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsPage : MonoBehaviour
{
    [SerializeField] Slider musicSlider, sfxSlider;
    [SerializeField] TMP_Text musicValue, sfxValue;
    private void OnEnable()
    {
        SetSoundSlider(musicSlider, musicValue, AudioManager.Instance.musicAudioSource);
        SetSoundSlider(sfxSlider, sfxValue, AudioManager.Instance.sfxAudioSource);
    }

    void SetSoundSlider(Slider slider, TMP_Text valueText,AudioSource audioSource)
    {
        slider.value = Mathf.Round(audioSource.volume * 100) / 100;
        valueText.text = slider.value * 100 + "%";
    }

    public void ChangeMusicVolume()
    {
        musicSlider.value = Mathf.Round(musicSlider.value * 100) / 100;
        AudioManager.Instance.SetMusicVolume(musicSlider.value);
        musicValue.text = musicSlider.value * 100 + "%";
    }

    public void ChangeSFXVolume()
    {
        sfxSlider.value = Mathf.Round(sfxSlider.value * 100) / 100;
        AudioManager.Instance.SetSFXVolume(sfxSlider.value);
        sfxValue.text = sfxSlider.value * 100 + "%";
    }
}
