using UnityEngine;
using UniRx;
using System.Collections.Generic;
using _00_Scripts.Game.Enemies;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private int _poolSize = 20;

    private Queue<Enemy> _enemyPool = new Queue<Enemy>();

    private void Awake()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            Enemy enemy = Instantiate(_enemyPrefab);
            enemy.gameObject.SetActive(false);
            _enemyPool.Enqueue(enemy);
        }
    }

    public Enemy GetEnemy(Vector2 position, Transform target)
    {
        if (_enemyPool.Count == 0)
        {
            // Если пул пуст, создаем нового врага
            Enemy newEnemy = Instantiate(_enemyPrefab);
            InitializeEnemy(newEnemy, position, target);
            return newEnemy;
        }

        Enemy enemy = _enemyPool.Dequeue();
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
        enemy.Initialize(target);
        enemy.gameObject.SetActive(true);

        enemy.OnDeath
            .Subscribe(_ => ReturnEnemy(enemy))
            .AddTo(this);
    }
}