using System;
using System.Collections;
using System.Collections.Generic;

using _00_Scripts.Events;
using _00_Scripts.Game.Enemies;
using _00_Scripts.Helpers;

using UnityEngine;

using Random = UnityEngine.Random;

namespace _00_Scripts.Game.Spawner
{
  public class EnemySpawner : MonoBehaviour
  {
    [Header("Settings")] [SerializeField] private EnemyPool _enemyPool;

    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _spawnInterval = 3f;
    [SerializeField] private int _maxEnemies = 10;
    [SerializeField] private float _spawnRadius = 5f;
    [SerializeField] private LayerMask _wallLayer;

    [Header("Wave Settings")] [SerializeField]
    private int _enemiesPerWave = 5;

    [SerializeField] private float _waveCooldown = 10f;
    [SerializeField] private float _spawnCheckRadius = 0.5f;

    private readonly List<Enemy> _activeEnemies = new();
    private int _currentWave = 1;
    private float _waveTimer;

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

    private void OnDrawGizmosSelected()
    {
      Gizmos.color = Color.green;
      Gizmos.DrawWireSphere(_playerTransform.position, _spawnRadius);
    }

    private void HandleWaveSpawning()
    {
      _waveTimer -= Time.deltaTime;

      if (_waveTimer <= 0)
      {
        _enemyPool.ChangeEnemyType(_currentWave);
        StartCoroutine(SpawnWave());
        _waveTimer = _waveCooldown;
        _currentWave++;
      }
    }

    private void CheckEnemiesCount()
    {
      _activeEnemies.RemoveAll(enemy => enemy == null || !enemy.gameObject.activeInHierarchy);

      if (_activeEnemies.Count == 0 && _enemyPool.PoolSize == 0)
      {
        Console.Write("Враги убиты");
        EventBus.Publish(new WavesEndEvent());
      }
    }

    private IEnumerator SpawnWave()
    {
      _activeEnemies.RemoveAll(enemy => enemy == null || !enemy.gameObject.activeInHierarchy);

      var enemiesToSpawn = _enemiesPerWave + _currentWave;

      for (var i = 0; i < enemiesToSpawn && _activeEnemies.Count < _maxEnemies; i++)
      {
        if (TrySpawnEnemy(out var enemy)) _activeEnemies.Add(enemy);
        yield return new WaitForSeconds(0.5f);
      }
    }

    private bool TrySpawnEnemy(out Enemy enemy)
    {
      enemy = null;
      var spawnPosition = GetValidSpawnPosition();

      if (spawnPosition != Vector2.zero)
      {
        enemy = _enemyPool.GetEnemy(spawnPosition, _playerTransform);
        return true;
      }

      return false;
    }

    private Vector2 GetValidSpawnPosition()
    {
      var attempts = 10;
      var spawnPosition = Vector2.zero;

      for (var i = 0; i < attempts; i++)
      {
        var randomDirection = Random.insideUnitCircle.normalized;
        spawnPosition = (Vector2)_playerTransform.position + randomDirection * _spawnRadius;

        if (!Physics2D.OverlapCircle(spawnPosition, _spawnCheckRadius, _wallLayer)) return spawnPosition;
      }

      return Vector2.zero;
    }
  }
}
