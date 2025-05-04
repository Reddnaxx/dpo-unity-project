using System.Collections.Generic;

using UnityEngine;

namespace _00_Scripts.Game.Enemies
{
  public class EnemyPool : MonoBehaviour
  {
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int poolSize = 10;
    private Queue<GameObject> enemyPool = new Queue<GameObject>();

    private void Awake()
    {
      InitializePool();
    }

    private void InitializePool()
    {
      for (int i = 0; i < poolSize; i++)
      {
        GameObject enemy = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
        enemy.SetActive(false);
        enemy.transform.SetParent(transform);
        enemyPool.Enqueue(enemy);
      }
    }

    public GameObject GetEnemy()
    {
      if (enemyPool.Count > 0)
      {
        GameObject enemy = enemyPool.Dequeue();
        return enemy;
      }
      else
      {
        GameObject enemy = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
        return enemy;
      }
    }

    public void ReturnEnemy(GameObject enemy)
    {
      enemy.SetActive(false);
      enemy.transform.SetParent(transform);
      enemyPool.Enqueue(enemy);
    }
  }
}
