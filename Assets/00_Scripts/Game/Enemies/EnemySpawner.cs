using UnityEngine;
using UniRx;
using System.Collections.Generic;
using _00_Scripts.Game.Enemies;

public class EnemySpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _spawnInterval = 3f;
    [SerializeField] private int _maxEnemies = 10;
    [SerializeField] private float _spawnRadius = 5f;

    [Header("Wave Settings")]
    [SerializeField] private int _enemiesPerWave = 5;
    [SerializeField] private float _waveCooldown = 10f;

    private List<Enemy> _activeEnemies = new List<Enemy>();
    private float _spawnTimer;
    private float _waveTimer;
    private int _currentWave = 1;

    private void Start()
    {
        _waveTimer = _waveCooldown;
    }

    private void Update()
    {
        HandleWaveSpawning();
    }

    private void HandleWaveSpawning()
    {
        _waveTimer -= Time.deltaTime;

        if (_waveTimer <= 0)
        {
            StartCoroutine(SpawnWave());
            _waveTimer = _waveCooldown;
            _currentWave++;
        }
    }

    private System.Collections.IEnumerator SpawnWave()
    {
        int enemiesToSpawn = _enemiesPerWave + _currentWave;

        for (int i = 0; i < enemiesToSpawn && _activeEnemies.Count < _maxEnemies; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f); 
        }
    }

    private void SpawnEnemy()
    {
        if (_activeEnemies.Count >= _maxEnemies) return;

        Vector2 spawnPosition = GetSpawnPosition();
        Enemy newEnemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
        newEnemy.Initialize(_playerTransform);

        newEnemy.OnDeath
            .Subscribe(_ => RemoveEnemy(newEnemy))
            .AddTo(this);

        _activeEnemies.Add(newEnemy);
    }

    private Vector2 GetSpawnPosition()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        return (Vector2)_playerTransform.position + randomDirection * _spawnRadius;
    }

    private void RemoveEnemy(Enemy enemy)
    {
        _activeEnemies.Remove(enemy);
        Destroy(enemy.gameObject);
    }
}