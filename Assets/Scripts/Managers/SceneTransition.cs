using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private bool _startHidden;

    public event Action OnHidden;
    public event Action OnShowed;

    private void Awake()
    {
        if (_startHidden)
        {
            Color color = _image.color;
            _image.color =new Color(color.r, color.g, color.b, 1f);
        }
    }

    public void HideScreen()
    {
        Color color = _image.color;
        Color transition = new Color(color.r, color.g, color.b, 1f);
        _image.DOColor(transition, 1f).OnComplete(() => OnHidden?.Invoke());
    }

    public void ShowScreen()
    {
        Color color = _image.color;
        Color transition = new Color(color.r, color.g, color.b, 0f);
        _image.DOColor(transition, 1f).OnComplete(() => OnShowed?.Invoke());
    }
}
