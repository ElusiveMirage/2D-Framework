using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SettingsManager : GenericSingleton<SettingsManager>
{
    private SettingsData settingsData;
    private string settingsFilePath;

    public float MasterVolume
    {
        get { return settingsData.masterVolume; }
        set { settingsData.masterVolume = value; }
    }

    public float MusicVolume
    {
        get { return settingsData.musicVolume; }
        set { settingsData.musicVolume = value; }
    }

    public float SFXVolume
    {
        get { return settingsData.sfxVolume; }
        set { settingsData.sfxVolume = value; }
    }

    public override void Awake()
    {
        base.Awake();

        settingsFilePath = Application.persistentDataPath + "/" + "settings.cfg";

        if (!File.Exists(settingsFilePath))
        {
            SettingsData data = new SettingsData();
            Utility.JsonSerializer.SaveJson(JsonUtility.ToJson(data), settingsFilePath);
        }

        LoadSettingsData();

    }

    public void LoadSettingsData()
    {
        if (File.Exists(settingsFilePath))
        {
            settingsData = new SettingsData();

            string jsonString = Utility.JsonSerializer.LoadJson(settingsFilePath);
            JsonUtility.FromJsonOverwrite(jsonString, settingsData);
        }
    }

    public void SaveSettingsData()
    {
        Utility.JsonSerializer.SaveJson(JsonUtility.ToJson(settingsData), settingsFilePath);
    }
}
