using Assets.Scripts.Domain;
using System;
using UnityEngine;

public class PowerUpsUI : MonoBehaviour
{
    [SerializeField] private PowerUpUI _powerUpUIPrefab;
    public event Action<APowerUp> OnPowerUpSelected;

    public void AddPowerUp(APowerUp powerUp)
    {
        PowerUpUI powerUpUi = Instantiate(_powerUpUIPrefab, transform);
        powerUpUi.SetPowerUp(powerUp);

        powerUpUi.OnSelect += (powerUp) =>
        {
            OnPowerUpSelected?.Invoke(powerUp);
        };
    }

    public void ClearPowerUps()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
