using UnityEngine;
using UnityEngine.EventSystems;

public class UISoundEffect : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    [SerializeField] private AudioClip _menuHover;
    [SerializeField] private AudioClip _menuClick;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_menuHover != null)
        {
            AudioManager.Instance.PlayAudio(
                _menuHover,
                AudioManager.SoundType.SFX,
                1.0f,
                false
            );
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_menuClick != null)
        {
            AudioManager.Instance.PlayAudio(
                _menuClick,
                AudioManager.SoundType.SFX,
                1.0f,
                false
            );
        }
    }
}