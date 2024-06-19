using UnityEngine;
using UnityEngine.UI;

public class VolumeSettingsUI : MonoBehaviour
{
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    private void Start()
    {
        // Set initial slider values to match the current settings
        musicVolumeSlider.value = SettingsManager.instance.GetMusicVolume() * 100; // Convert from 0-1 to 0-100
        sfxVolumeSlider.value = SettingsManager.instance.GetSFXVolume() * 100; // Convert from 0-1 to 0-100

        // Add listeners to handle slider value changes
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    private void OnDestroy()
    {
        // Remove listeners when the object is destroyed
        musicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.RemoveListener(OnSFXVolumeChanged);
    }

    private void OnMusicVolumeChanged(float volume)
    {
        SettingsManager.instance.SetMusicVolume(volume / 100f); // Convert from 0-100 to 0-1
    }

    private void OnSFXVolumeChanged(float volume)
    {
        SettingsManager.instance.SetSFXVolume(volume / 100f); // Convert from 0-100 to 0-1
    }
}
