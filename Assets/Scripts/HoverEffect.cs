using UnityEngine;
using UnityEngine.EventSystems;

public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] float _hoverScaleIncrease = 1.1f;
    [SerializeField] float _clickScaleIncrease = 1.3f;
    [SerializeField] float _tweenEffectDuration = 0.1f;

    public void OnPointerDown(PointerEventData eventData)
    {
        LeanTween.cancel(gameObject);
        transform.localScale = Vector2.one * _clickScaleIncrease;
        LeanTween.scale(gameObject, Vector2.one * _hoverScaleIncrease, _tweenEffectDuration).setIgnoreTimeScale(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector2.one * _hoverScaleIncrease, _tweenEffectDuration).setIgnoreTimeScale(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector2.one, _tweenEffectDuration).setIgnoreTimeScale(true);
    }
}
