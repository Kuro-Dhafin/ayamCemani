using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;

    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";

    private float musicVolume = 1f;
    private float sfxVolume = 1f;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        if (instance == null)
        {
            GameObject settingsManagerObject = new GameObject("SettingsManager");
            instance = settingsManagerObject.AddComponent<SettingsManager>();
            DontDestroyOnLoad(settingsManagerObject);
            instance.LoadSettings();
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Load saved settings from player preferences
    private void LoadSettings()
    {
        musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 1f);
        sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);
    }

    // Apply settings to AudioManager
    public void ApplySettings()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetMusicVolume(musicVolume);
            AudioManager.instance.SetSFXVolume(sfxVolume);
        }
    }

    // Save current settings to player preferences
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, musicVolume);
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, sfxVolume);
        PlayerPrefs.Save();
    }

    // Setters for volume control
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        AudioManager.instance?.SetMusicVolume(volume);
        SaveSettings();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        AudioManager.instance?.SetSFXVolume(volume);
        SaveSettings();
    }

    // Getters for volume control
    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public float GetSFXVolume()
    {
        return sfxVolume;
    }
}
