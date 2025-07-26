using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    [SerializeField] private Sprite _filled;
    [SerializeField] private Sprite _empty;
    [SerializeField] private Image _image;

    public void Fill()
    {
        Sequence fillAnimation = DOTween.Sequence();
        fillAnimation.Append(transform.DOScale(1.2f, .5f));
        fillAnimation.Append(transform.DOScale(1f, .5f));
        _image.sprite = _filled;
    }

    public void Empty()
    {
        transform.DOShakePosition(1f, 20);
        _image.sprite = _empty;
    }
}
