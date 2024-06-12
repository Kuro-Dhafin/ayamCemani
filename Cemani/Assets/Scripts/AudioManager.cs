using UnityEngine;

public class AudioManager : MonoBehaviour
{
  [Header("-----------AudioSource------------")]//Sumber Audio
  [SerializeField] AudioSource musicSource;
  [SerializeField] AudioSource SFXSource;


  [Header("-----------AudioClip------------")] //Sumber SFX
  public AudioClip background;
  public AudioClip ButtonClick;
  public AudioClip MouseHover;

  private void Start()//Script untuk mulai music di Menu
  {
    musicSource.clip = background;
    musicSource.loop = true;
    musicSource.Play();
  }

}
