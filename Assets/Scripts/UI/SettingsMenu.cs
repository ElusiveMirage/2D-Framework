using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {

    }

    public void OnMasterVolumeSliderChanged(float volume)
    {

    }

    public void OnMusicVolumeSliderChanged(float volume)
    {

    }

    public void OnSFXVolumeSliderChanged(float volume)
    {

    }

    public void Apply()
    {
        SettingsManager.Instance.MasterVolume = masterVolumeSlider.value;
        SettingsManager.Instance.MusicVolume = musicVolumeSlider.value;
        SettingsManager.Instance.SFXVolume = sfxVolumeSlider.value;
        SettingsManager.Instance.SaveSettingsData();
    }

    private void Initialize()
    {
        if(SettingsManager.Instance != null)
        {
            masterVolumeSlider.value = SettingsManager.Instance.MasterVolume;
            musicVolumeSlider.value = SettingsManager.Instance.MusicVolume;
            sfxVolumeSlider.value = SettingsManager.Instance.SFXVolume;          
        }     
    }
}
