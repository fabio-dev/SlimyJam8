using UnityEngine;
using System.Collections;
using Assets.Scripts.Domain;
using System.Linq;
using System;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private SpawnPhase[] _phases;
    [SerializeField] private float _spawnRadiusMin = 10f;
    [SerializeField] private float _spawnRadiusMax = 14f;

    private PlayerGO _player;
    private DropManager _dropManager;
    private ScoreManager _scoreManager;
    private SpawnPhase _currentPhase;
    private int _currentPhaseIndex = -1;
    private Cooldown _nextPhaseCooldown = new Cooldown(0f);
    private float _delayToNextSpawn = 0f;
    private bool _started;

    public void Setup(PlayerGO player, DropManager dropManager, ScoreManager scoreManager)
    {
        _player = player;
        _dropManager = dropManager;
        _scoreManager = scoreManager;
        _started = true;

        SetNextPhase();
        StartCoroutine(SpawnLoop());
    }

    private void Update()
    {
        if (_started && !_nextPhaseCooldown.IsRunning())
        {
            SetNextPhase();
        }
    }

    private void SetNextPhase()
    {
        _currentPhaseIndex = Math.Min(_currentPhaseIndex + 1, _phases.Length - 1);
        _currentPhase = _phases[_currentPhaseIndex];

        _nextPhaseCooldown.SetDuration(_currentPhase.PhaseDurationInSeconds);
        _nextPhaseCooldown.Start();
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(_delayToNextSpawn);
        }
    }

    private void SpawnEnemy()
    {
        if (_player == null)
        {
            return;
        }

        Vector2 randomDir = UnityEngine.Random.insideUnitCircle.normalized;
        float randomDistance = UnityEngine.Random.Range(_spawnRadiusMin, _spawnRadiusMax);
        Vector2 spawnPosition = (Vector2)_player.transform.position + randomDir * randomDistance;

        EnemyGO enemyPrefab = RandomEnemy();
        EnemyGO enemyGO = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        Enemy enemy = new Enemy();
        enemyGO.Setup(enemy);
        enemyGO.SetDropManager(_dropManager);
        enemy.OnDie += UpdateScore;

        _delayToNextSpawn = UnityEngine.Random.Range(_currentPhase.MinDelayToSpawnInSeconds, _currentPhase.MaxDelayToSpawnInSeconds);
    }

    private void UpdateScore(ACharacter enemy)
    {
        _scoreManager.AddScore(((Enemy)enemy).Score);
        enemy.OnDie -= UpdateScore;
    }

    private EnemyGO RandomEnemy()
    {
        if (_currentPhase.Enemies.Length == 1)
        {
            return _currentPhase.Enemies[0].Enemy;
        }

        int totalWeight = _currentPhase.Enemies.Sum(e => e.Weight);
        int rng = UnityEngine.Random.Range(0, totalWeight);
        int lookingWeight = 0;

        foreach (SpawnEnemy enemy in _currentPhase.Enemies)
        {
            lookingWeight += enemy.Weight;
            if (rng < lookingWeight)
            {
                return enemy.Enemy;
            }
        }

        return _currentPhase.Enemies.Last().Enemy;
    }
}