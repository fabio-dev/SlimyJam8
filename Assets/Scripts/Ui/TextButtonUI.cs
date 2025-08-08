using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Color _textColor;
    [SerializeField] private Color _hoverColor;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _text.DOColor(_hoverColor, .3f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _text.DOColor(_textColor, .3f);
    }

}
