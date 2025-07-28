using Assets.Scripts.Domain;
using Assets.Scripts.Domain.PowerUps;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PowerUpUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private Sprite _damageSprite;
    [SerializeField] private Sprite _supportSprite;
    [SerializeField] private Sprite _flaskSprite;

    private Color _baseColor;
    private APowerUp _powerUp;

    public event Action<APowerUp> OnSelect;

    void Start()
    {
        if (_image == null)
        {
            _image = GetComponent<Image>();
        }

        _baseColor = _image.color;
    }

    public void SetPowerUp(APowerUp powerUp)
    {
        _powerUp = powerUp;
        _title.text = _powerUp.Title;

        if (_powerUp.PowerUpType == PowerUpType.Damage)
        {
            _image.sprite = _damageSprite;
        }
        else if (_powerUp.PowerUpType == PowerUpType.Support)
        {
            _image.sprite = _supportSprite;
        }
        else
        {
            _image.sprite = _flaskSprite;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnSelect?.Invoke(_powerUp);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _image.color = Color.gray;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _image.color = _baseColor;
    }
}