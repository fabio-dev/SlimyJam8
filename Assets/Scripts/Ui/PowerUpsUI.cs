using Assets.Scripts.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerUpsUI : MonoBehaviour
{
    [SerializeField] private PowerUpUI _powerUpUIPrefab;
    private List<PowerUpUI> _powerUpUIList = new();
    private PowerUpUI _currentSelectedPowerUp;
    private int _currentPowerUpIndex = -1;

    public event Action<APowerUp> OnPowerUpSelected;

    public void AddPowerUp(APowerUp powerUp)
    {
        PowerUpUI powerUpUi = Instantiate(_powerUpUIPrefab, transform);
        powerUpUi.SetPowerUp(powerUp);

        powerUpUi.OnSelect += SelectPowerUp;
        powerUpUi.OnEnter += EnterPowerUp;
        powerUpUi.OnExit += ExitPowerUp;

        _powerUpUIList.Add(powerUpUi);
    }

    private void InputNavigatePerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (_powerUpUIList == null || _powerUpUIList.Count <= 0)
        {
            return;
        }

        Vector2 move = context.ReadValue<Vector2>();

        if (move.x > 0f)
        {
            EnterNextPowerUp();
        }

        if (move.x < 0f)
        {
            EnterPreviousPowerUp();
        }
    }

    private void InputSubmitPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (_powerUpUIList == null || _powerUpUIList.Count <= 0 || _currentSelectedPowerUp == null)
        {
            return;
        }

        SelectPowerUp(_currentSelectedPowerUp);
    }

    private void EnterNextPowerUp()
    {
        _currentPowerUpIndex++;

        if (_currentPowerUpIndex >= _powerUpUIList.Count)
        {
            _currentPowerUpIndex = 0;
        }

        EnterPowerUp(_powerUpUIList[_currentPowerUpIndex]);
    }

    private void EnterPreviousPowerUp()
    {
        _currentPowerUpIndex--;

        if (_currentPowerUpIndex < 0)
        {
            _currentPowerUpIndex = _powerUpUIList.Count - 1;
        }

        EnterPowerUp(_powerUpUIList[_currentPowerUpIndex]);
    }

    private void SelectPowerUp(PowerUpUI powerUp)
    {
        OnPowerUpSelected?.Invoke(powerUp.PowerUp);
        ResetCurrentPowerUp();
    }

    private void ResetCurrentPowerUp()
    {
        _currentPowerUpIndex = -1;
        _currentSelectedPowerUp = null;
    }

    private void EnterPowerUp(PowerUpUI powerUp)
    {
        foreach (PowerUpUI otherPowerUpUI in _powerUpUIList.Where(p => p != powerUp))
        {
            otherPowerUpUI.SetExited();
        }

        _currentSelectedPowerUp = powerUp;
        powerUp.SetEntered();
    }

    private void ExitPowerUp(PowerUpUI powerUp)
    {
        if (_currentSelectedPowerUp == powerUp)
        {
            _currentSelectedPowerUp = null;
        }
        powerUp.SetExited();
    }

    public void ClearPowerUps()
    {
        ResetCurrentPowerUp();
        _powerUpUIList.Clear();

        foreach (PowerUpUI powerUpUi in _powerUpUIList)
        {
            powerUpUi.OnSelect -= SelectPowerUp;
            powerUpUi.OnEnter -= EnterPowerUp;
            powerUpUi.OnExit -= ExitPowerUp;
        }

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

    private void OnDisable()
    {
        InputManager.Instance.UI.Value.Navigate.performed -= InputNavigatePerformed;
        InputManager.Instance.UI.Value.Submit.performed -= InputSubmitPerformed;
    }

    private void OnEnable()
    {
        InputManager.Instance.UI.Value.Navigate.performed += InputNavigatePerformed;
        InputManager.Instance.UI.Value.Submit.performed += InputSubmitPerformed;
    }
}
