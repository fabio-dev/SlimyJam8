using Assets.Scripts.Domain;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private APowerUp[] availablePowerUps;

    public List<APowerUp> GetRandomPowerUps(int count)
    {
        var selectedPowerUps = new List<APowerUp>();
        var pool = new List<APowerUp>(availablePowerUps);

        for (int i = 0; i < count && pool.Count > 0; i++)
        {
            int totalWeight = pool.Sum(p => p.DropWeight);
            int randomWeight = Random.Range(0, totalWeight);

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
}
