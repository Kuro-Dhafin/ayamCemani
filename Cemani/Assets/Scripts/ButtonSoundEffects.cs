using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSoundEffects : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.instance.MH(AudioManager.instance.MouseHover);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.instance.BC(AudioManager.instance.ButtonClick);
    }
}
