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
        _image.sprite = _filled;
    }

    public void Empty()
    {
        transform.DOShakePosition(1f, 20);
        _image.sprite = _empty;
    }
}
