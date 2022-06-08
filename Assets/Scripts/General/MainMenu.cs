using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string startingScene;
    [SerializeField] private GameObject settingsMenuCanvas;
    [SerializeField] private AudioClip menuBGM;

    private void Start()
    {
        SoundManager.Instance.PlayBGM(menuBGM);
    }

    public void StartGame()
    {
        SoundManager.Instance.PlaySound(Resources.Load<AudioClip>("SFX/SFX_UI_Confirm"));
        SceneLoader.Instance.LoadScene(startingScene);
    }

    public void OpenSettings()
    {
        SoundManager.Instance.PlaySound(Resources.Load<AudioClip>("SFX/SFX_UI_OpenMenu"));
        settingsMenuCanvas.SetActive(true);
    }

    public void CloseSettings()
    {
        SoundManager.Instance.PlaySound(Resources.Load<AudioClip>("SFX/SFX_UI_CloseMenu"));
        settingsMenuCanvas.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
