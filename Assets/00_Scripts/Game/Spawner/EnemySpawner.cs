using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using _00_Scripts.Events;
using _00_Scripts.Game.Enemies;
using _00_Scripts.Helpers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _00_Scripts.Game.Spawner
{
  public class EnemySpawner : MonoBehaviour
  {
    [Header("Settings")]
    [SerializeField] private EnemyPool _enemyPool;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _spawnInterval = 3f;
    [SerializeField] private int _maxEnemies = 10;
    [SerializeField] private float _spawnRadius = 5f;
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private int _maxWaves = 3;

    [Header("Wave Settings")]
    [SerializeField] private int _enemiesPerWave = 5;
    [SerializeField] private float _waveCooldown = 10f;
    [SerializeField] private float _spawnCheckRadius = 0.5f;

    private readonly List<Enemy> _activeEnemies = new();
    private int _currentWave = 0;
    private int _enemiesSpawnedInCurrentWave = 0;
    private float _waveTimer;
    private bool _waveInProgress = false;

    private void Start()
    {
      _waveTimer = _waveCooldown;
      _wallLayer = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
      HandleWaveSpawning();
      CheckEnemiesCount();
    }

    private void HandleWaveSpawning()
    {
      if (_currentWave >= _maxWaves) return;

      _waveTimer -= Time.deltaTime;

      if (_waveTimer <= 0 && !_waveInProgress)
      {
        StartCoroutine(SpawnWave());
      }
    }

    private IEnumerator SpawnWave()
    {
      _waveInProgress = true;
      _enemiesSpawnedInCurrentWave = 0;
      _enemyPool.ChangeEnemyType(_currentWave);

      int enemiesToSpawn = Mathf.Min(_enemiesPerWave, _maxEnemies - _activeEnemies.Count);

      for (int i = 0; i < enemiesToSpawn; i++)
      {
        if (TrySpawnEnemy(out var enemy))
        {
          _activeEnemies.Add(enemy);
          _enemiesSpawnedInCurrentWave++;

          enemy.OnDeath
            .Subscribe(_ => _activeEnemies.Remove(enemy))
            .AddTo(enemy);
        }
        yield return new WaitForSeconds(_spawnInterval);
      }

      _currentWave++;
      _waveTimer = _waveCooldown;
      _waveInProgress = false;
    }

    private void CheckEnemiesCount()
    {
      if (_currentWave >= _maxWaves && _activeEnemies.Count == 0)
      {
        EventBus.Publish(new WavesEndEvent());
        enabled = false; // Отключаем спавнер после завершения всех волн
      }
    }

    private bool TrySpawnEnemy(out Enemy enemy)
    {
      enemy = null;
      Vector2 spawnPosition = GetValidSpawnPosition();

      if (spawnPosition != Vector2.zero)
      {
        enemy = _enemyPool.GetEnemy(spawnPosition, _currentWave, _playerTransform);
        return true;
      }
      return false;
    }

    private Vector2 GetValidSpawnPosition()
    {
      for (int i = 0; i < 10; i++)
      {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector2 spawnPosition = (Vector2)_playerTransform.position + randomDirection * _spawnRadius;

        if (!Physics2D.OverlapCircle(spawnPosition, _spawnCheckRadius, _wallLayer))
        {
          return spawnPosition;
        }
      }
      return Vector2.zero;
    }

    private void OnDrawGizmosSelected()
    {
      Gizmos.color = Color.green;
      Gizmos.DrawWireSphere(_playerTransform.position, _spawnRadius);
    }
  }
}
