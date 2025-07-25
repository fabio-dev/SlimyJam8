using Assets.Scripts.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] private APowerUp[] _availablePowerUps;
    [SerializeField] private PowerUpsUI _powerUpsUI;
    private Player _player;
    private LevelManager _levelManager;

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
        StartCoroutine(ShowPowerUpsUI());
    }

    private IEnumerator ShowPowerUpsUI()
    {
        yield return new WaitForSeconds(1f);

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

        OnSelected?.Invoke();
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
