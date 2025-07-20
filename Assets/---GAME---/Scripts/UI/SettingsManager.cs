using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Slider soundSlider = null;
    [SerializeField] private Slider musicSlider = null;
    
    [SerializeField] private List<Toggle> checkboxes = null;
    
    [SerializeField] private AudioMixer soundMixer = null;
    [SerializeField] private AudioMixer musicMixer = null;
    [SerializeField] private GameObject settingsPanel = null;
    
    private void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            QualitySettings.SetQualityLevel(3);
            soundSlider.value = 0.8f;
            musicSlider.value =  0.8f;
        }
        
        ResetCheckboxes(QualitySettings.GetQualityLevel());
    }

    public void OnSettingsButtonPressed()
    {
        settingsPanel.SetActive(!settingsPanel.activeInHierarchy);
    }
    
    public void SetLowQuality(bool toggle)
    {
        if (!toggle) return;
        QualitySettings.SetQualityLevel(3);
        ResetCheckboxes(0);
    }
    
    public void SetMediumQuality(bool toggle)
    {
        if (!toggle) return;
        QualitySettings.SetQualityLevel(3);
        ResetCheckboxes(1);
    }
    
    public void SetHighQuality(bool toggle)
    {
        if (!toggle) return;
        QualitySettings.SetQualityLevel(3);
        ResetCheckboxes(2);
    }
    
    public void SetVeryHighQuality(bool toggle)
    {
        if (!toggle) return;
        QualitySettings.SetQualityLevel(4);
        ResetCheckboxes(3);
    }
    
    public void SetUltraQuality(bool toggle)
    {
        if (!toggle) return;
        QualitySettings.SetQualityLevel(5);
        ResetCheckboxes(4);
    }

    private void ResetCheckboxes(int excludeIndex)
    {
        checkboxes[excludeIndex].isOn = true;
        for (int i = 0; i < checkboxes.Count; i++)
        {
            if (i == excludeIndex) continue;
            checkboxes[i].isOn = false;
        }
    }

    public void SetSoundVolume(float volume)
    {
        Debug.Log(volume);
        soundMixer.SetFloat("Volume",  Mathf.Lerp(-80,20,volume));
    }
    
    public void SetMusicVolume(float volume)
    {
        Debug.Log(volume);
        musicMixer.SetFloat("Volume", Mathf.Lerp(-80,20,volume));
    }
}
