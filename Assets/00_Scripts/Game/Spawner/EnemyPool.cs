using System.Collections.Generic;

using _00_Scripts.Game.Enemies;

using UniRx;

using UnityEngine;

namespace _00_Scripts.Game.Spawner
{
  public class EnemyPool : MonoBehaviour
  {
    [SerializeField] private List<Enemy> _enemyPrefabs; // Теперь список префабов
    [SerializeField] private int _poolSize = 40;

    private readonly Queue<Enemy> _enemyPool = new();
    private int _currentPrefabIndex;

    public int PoolSize { get; private set; } = int.MaxValue;

    private void Awake()
    {
      InitializePool();
      PoolSize = _poolSize;
    }

    private void InitializePool()
    {
      _enemyPool.Clear();

      for (var i = 0; i < _poolSize; i++)
      {
        var enemy = Instantiate(_enemyPrefabs[_currentPrefabIndex], transform);
        enemy.gameObject.SetActive(false);

        enemy.OnDeath
          .Subscribe(_ => ReturnEnemy(enemy))
          .AddTo(enemy);

        _enemyPool.Enqueue(enemy);

        PoolSize--;
      }
    }

    public void ChangeEnemyType(int waveNumber)
    {
      // Выбираем префаб на основе номера волны (можно изменить логику выбора)
      _currentPrefabIndex = waveNumber % _enemyPrefabs.Count;
      InitializePool(); // Переинициализируем пул с новым типом врагов
    }

    public Enemy GetEnemy(Vector2 position, int _waveIndex, Transform target)
    {
      if (_enemyPool.Count == 0)
      {
        var newEnemy = Instantiate(_enemyPrefabs[_waveIndex - 1], transform);
        newEnemy.OnDeath
          .Subscribe(_ => ReturnEnemy(newEnemy))
          .AddTo(newEnemy);

        InitializeEnemy(newEnemy, position, target);
        return newEnemy;
      }

      var enemy = _enemyPool.Dequeue();
      InitializeEnemy(enemy, position, target);
      return enemy;
    }

    public void ReturnEnemy(Enemy enemy)
    {
      enemy.gameObject.SetActive(false);
      _enemyPool.Enqueue(enemy);
    }

    private void InitializeEnemy(Enemy enemy, Vector2 position, Transform target)
    {
      enemy.transform.position = position;
      enemy.gameObject.SetActive(true);
    }
  }
}
