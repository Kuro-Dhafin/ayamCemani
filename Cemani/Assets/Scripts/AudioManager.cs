using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("-----------AudioSource------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("-----------AudioClip------------")]
    public AudioClip background;
    public AudioClip ButtonClick;
    public AudioClip MouseHover;
    public AudioClip Shoot;
    public AudioClip Walk;
    public AudioClip hurt;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        musicSource.clip = background;
        musicSource.loop = true;
        musicSource.Play();
        SettingsManager.instance?.ApplySettings();  // Use null-conditional operator
    }

    public void BC(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void MH(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void PlayHurt(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void PlayShoot()
    {
        SFXSource.PlayOneShot(Shoot);
    }

    public void PlayWalk()
    {
        SFXSource.PlayOneShot(Walk);
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        SFXSource.volume = volume;
    }
}
