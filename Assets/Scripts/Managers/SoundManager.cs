using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : GenericSingleton<SoundManager>
{
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private bool playBGM;
    [SerializeField] private bool playSFX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bgmSource.volume = SettingsManager.Instance.MasterVolume * SettingsManager.Instance.MusicVolume;
        sfxSource.volume = SettingsManager.Instance.MasterVolume * SettingsManager.Instance.SFXVolume;
    }

    public void PlayBGM(AudioClip clip)
    {
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PlaySound(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlaySound(AudioClip clip, Vector3 position)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void ToggleMusic()
    {
        bgmSource.mute = !playBGM;
    }
    public void ToggleEffects()
    {
        sfxSource.mute = !playSFX;
    }
}
