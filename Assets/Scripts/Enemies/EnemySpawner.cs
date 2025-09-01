using Assets.Scripts.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private SpawnPhase[] _phases;
    [SerializeField] private DungeonGenerator _dungeon;

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
            SpawnEnemies();
            yield return new WaitForSeconds(_delayToNextSpawn);
        }
    }

    private void SpawnEnemies()
    {
        if (_player == null)
        {
            return;
        }

        ChunkGO chunk = _dungeon.GetPlayerCurrentChunk();
        List<SpawnAreaGO> spawnAreas = new List<SpawnAreaGO>(chunk.SpawnAreas);

        for (int i = 0; i < _currentPhase.NumberOfEnemiesToSpawn; i++)
        {
            if (spawnAreas.Count == 0)
            {
                break;
            }

            Debug.Log($"Spawning enemies in chunk [{chunk.name}]");
            int rngArea = UnityEngine.Random.Range(0, spawnAreas.Count);

            EnemyGO enemyPrefab = RandomEnemy();
            EnemyGO enemyGO = Instantiate(enemyPrefab, spawnAreas[rngArea].transform.position, Quaternion.identity);

            Enemy enemy = new Enemy();
            enemyGO.Setup(enemy);
            enemyGO.SetDropManager(_dropManager);
            enemy.OnDie += UpdateScore;
        }

        _delayToNextSpawn = _currentPhase.DelayToNextSpawnInSeconds;
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