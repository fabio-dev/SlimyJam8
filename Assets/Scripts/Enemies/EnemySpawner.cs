using UnityEngine;
using System.Collections;
using Assets.Scripts.Domain;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyGO[] _enemyPrefabs;
    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private float _spawnRadiusMin = 8f;
    [SerializeField] private float _spawnRadiusMax = 12f;

    private PlayerGO _player;
    private DropManager _dropManager;
    private ScoreManager _scoreManager;

    public void Setup(PlayerGO player, DropManager dropManager, ScoreManager scoreManager)
    {
        _player = player;
        _dropManager = dropManager;
        _scoreManager = scoreManager;

        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        if (_player == null)
        {
            return;
        }

        Vector2 randomDir = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(_spawnRadiusMin, _spawnRadiusMax);
        Vector2 spawnPosition = (Vector2)_player.transform.position + randomDir * randomDistance;

        EnemyGO enemyPrefab = RandomEnemy();
        EnemyGO enemyGO = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        Enemy enemy = new Enemy();
        enemyGO.Setup(enemy);
        enemyGO.SetDropManager(_dropManager);
        enemy.OnDie += UpdateScore;
    }

    private void UpdateScore(ACharacter enemy)
    {
        _scoreManager.AddScore(((Enemy)enemy).Score);
        enemy.OnDie -= UpdateScore;
    }

    private EnemyGO RandomEnemy()
    {
        int rng = Random.Range(0, _enemyPrefabs.Length);
        return _enemyPrefabs[rng];
    }
}