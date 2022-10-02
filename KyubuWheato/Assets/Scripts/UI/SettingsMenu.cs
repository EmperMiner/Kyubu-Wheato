using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using System;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown graphicsDropdown;
    [SerializeField] private TMP_Dropdown screenSizeDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private AudioMixer SFXMixer;
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private AudioMixer MusicMixer;
    private AudioManager AudioPlayer;

    [SerializeField] private Resolution[] resolutions;

    private void Start() 
    { 
        StartingSettings();
    }

    public void ConfirmSettings()
    {
        PlayerPrefs.SetInt("ResolutionIndex", screenSizeDropdown.value);
        PlayerPrefs.SetInt("QualityIndex", graphicsDropdown.value);
        PlayerPrefs.SetInt("FullscreenToggle", Convert.ToInt32(fullscreenToggle.isOn));
        PlayerPrefs.SetFloat("SFXVolume", SFXSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", MusicSlider.value);
        ApplyChanges();
    }

    private void ApplyChanges()
    {
        screenSizeDropdown.value = PlayerPrefs.GetInt("ResolutionIndex");
        graphicsDropdown.value = PlayerPrefs.GetInt("QualityIndex");
        fullscreenToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenToggle"));
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume");

        Resolution resolution = resolutions[PlayerPrefs.GetInt("ResolutionIndex")];
        Screen.SetResolution(resolution.width, resolution.height, Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenToggle")));
        Screen.fullScreen = Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenToggle"));
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("QualityIndex"));
        SFXMixer.SetFloat("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume")) * 20);
        MusicMixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
    }

    public void ButtonSelect()
    {
        AudioPlayer.PlaySound("UIButtonPress");
    }

    public void StartingSettings() 
    {
        AudioPlayer = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        resolutions = Screen.resolutions;
        screenSizeDropdown.ClearOptions();

        List<string> options = new List<String>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height + " @" + resolutions[i].refreshRate + "Hz";;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        screenSizeDropdown.AddOptions(options);
        ApplyChanges();
        screenSizeDropdown.value = currentResolutionIndex;
        screenSizeDropdown.RefreshShownValue();
    }
}
