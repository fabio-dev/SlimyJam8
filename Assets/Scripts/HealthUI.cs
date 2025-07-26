using Assets.Scripts.Domain;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private HeartUI _heartPrefab;
    private List<HeartUI> _hearts = new List<HeartUI>();
    private Player _player;
    private int _healthMarker;

    public void Setup(Player player)
    {
        _player = player;

        for (int i = 0; i < (int)player.Health.BaseValue; i++)
        {
            HeartUI heart = Instantiate(_heartPrefab, transform);
            _hearts.Add(heart);
            heart.Fill();
        }
        _healthMarker = (int)player.Health.BaseValue - 1;

        _player.OnDamaged += OnDamaged;
        _player.OnHealed += OnHealed;
    }

    private void OnDamaged(float damages)
    {
        int damagesAsInt = (int)damages;
        while (_healthMarker >= 0 && damagesAsInt > 0)
        {
            _hearts[_healthMarker].Empty();
            _healthMarker--;
            damagesAsInt--;
        }
    }

    private void OnHealed(float heal)
    {
        int healAsInt = (int)heal;
        while (_healthMarker < _hearts.Count && healAsInt > 0)
        {
            _healthMarker++;
            _hearts[_healthMarker].Fill();
            healAsInt--;
        }
    }

    private void OnDestroy()
    {
        _player.OnDamaged -= OnDamaged;
    }
}
