using Assets.Scripts.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] private APowerUp[] _availablePowerUps;
    [SerializeField] private PowerUpsUI _powerUpsUI;
    private Player _player;
    private LevelManager _levelManager;
    private int _powerUpsToChoose = 0;
    private bool _isChoosing = false;

    public event Action OnSelecting;
    public event Action OnSelected;

    private void Start()
    {
        _powerUpsUI.OnPowerUpSelected += OnPowerUpSelected;
        _powerUpsUI.Hide();
    }

    public void Setup(Player player, LevelManager levelManager)
    {
        _player = player;
        _levelManager = levelManager;
        _levelManager.OnLevelUp += LevelUp;
    }

    private void LevelUp()
    {
        _powerUpsToChoose++;
        StartCoroutine(ShowPowerUpsUI());
    }

    private IEnumerator ShowPowerUpsUI()
    {
        if (!_isChoosing)
        {
            _isChoosing = true;
            yield return new WaitForSeconds(.2f);
            SelectPowerUp();
        }
    }

    private void SelectPowerUp()
    {
        OnSelecting?.Invoke();

        _powerUpsUI.Show();
        List<APowerUp> powerUps = GetRandomPowerUps(3);

        foreach (APowerUp powerUp in powerUps)
        {
            _powerUpsUI.AddPowerUp(powerUp);
        }
    }

    private void OnPowerUpSelected(APowerUp powerUp)
    {
        if (_player == null)
        {
            return;
        }

        powerUp.Use(_player);
        _powerUpsUI.ClearPowerUps();
        _powerUpsUI.Hide();

        _powerUpsToChoose--;

        if (_powerUpsToChoose > 0)
        {
            SelectPowerUp();
        }
        else
        {
            OnSelected?.Invoke();
            _isChoosing = false;
        }
    }

    private List<APowerUp> GetRandomPowerUps(int count)
    {
        var selectedPowerUps = new List<APowerUp>();
        var pool = new List<APowerUp>(_availablePowerUps);

        for (int i = 0; i < count && pool.Count > 0; i++)
        {
            int totalWeight = pool.Sum(p => p.DropWeight);
            int randomWeight = UnityEngine.Random.Range(0, totalWeight);

            int current = 0;
            foreach (var powerUp in pool)
            {
                current += powerUp.DropWeight;
                if (randomWeight < current)
                {
                    selectedPowerUps.Add(powerUp);
                    pool.Remove(powerUp);
                    break;
                }
            }
        }

        return selectedPowerUps;
    }

    private void OnDestroy()
    {
        _levelManager.OnLevelUp -= LevelUp;
    }
}
