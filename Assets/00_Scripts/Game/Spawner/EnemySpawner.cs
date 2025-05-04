using UnityEngine;

namespace _00_Scripts.Game.Enemies
{
  public class EnemySpawner : MonoBehaviour
  {
    [SerializeField] private EnemyPool enemyPool;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float spawnRadius = 5f;
    private float timer;

    private void Update()
    {
      timer += Time.deltaTime;
      if (timer >= spawnInterval)
      {
        SpawnEnemy();
        timer = 0f;
      }
    }

    private void SpawnEnemy()
    {
      GameObject enemyObj = enemyPool.GetEnemy();
      if (enemyObj == null) return;

      Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;
      Vector3 spawnPosition = transform.position + new Vector3(randomPoint.x, randomPoint.y, 0);

      Enemy enemy = enemyObj.GetComponent<Enemy>();
      enemy.Activate(spawnPosition);
    }

    private void OnDrawGizmos()
    {
      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
  }
}
