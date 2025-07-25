using Assets.Scripts.Domain;
using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _intervalToGet1Score;
    private int _score;
    private Player _player;

    public void Setup(Player player)
    {
        _player = player;
    }

    public void Run()
    {
        StartCoroutine(ScoreTickLoop());
    }

    public void AddScore(int amount)
    {
        if (_player == null || _player.Health.IsDead())
        {
            return;
        }

        _score += amount;
        UpdateUI();
    }

    private IEnumerator ScoreTickLoop()
    {
        while (_player?.Health?.IsDead() == false)
        {
            yield return new WaitForSeconds(_intervalToGet1Score);
            AddScore(1);
        }
    }

    private void UpdateUI()
    {
        _text.text = _score.ToString();
    }
}
