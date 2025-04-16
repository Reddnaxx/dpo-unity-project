using System.Linq;
using System;
using _00_Scripts.Game.Enemies;
using UniRx;
using UnityEngine;

namespace _00_Scripts.Game.Weapon
{
  public class WeaponMagicBook : Weapon
  {
    [Header("Magic Book Settings")]

    // Props
    [SerializeField]
    private Transform playerTransform;

    [SerializeField] private float attackRadius = 5f;
    [SerializeField] private float orbitDistance = 1.5f;
    [SerializeField] private float orbitSpeed = 90f;
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private LayerMask enemyLayer;

    private Vector2 _currentPosition;
    private float _currentAngle;

    protected override void Start()
    {
      playerTransform = transform.parent;
      _currentPosition = transform.position;
      if (playerTransform == null)
      {
        Debug.LogError("WeaponMagicBook must be a child of the player!");
        return;
      }

      Observable.Interval(TimeSpan.FromSeconds(fireRate))
        .Subscribe(_ => TryShootAtNearestEnemy())
        .AddTo(this);

      _currentAngle = 0f;
    }

    protected override void Update()
    {
      _currentAngle += orbitSpeed * Time.deltaTime;
      var offset = new Vector2(Mathf.Cos(_currentAngle * Mathf.Deg2Rad),
        Mathf.Sin(_currentAngle * Mathf.Deg2Rad)) * orbitDistance;

      _currentPosition = Vector2.Lerp(_currentPosition,
        playerTransform.position, Time.deltaTime * followSpeed);

      transform.position = _currentPosition + offset;
    }

    private void TryShootAtNearestEnemy()
    {
      var colliders = Physics2D.OverlapCircleAll(playerTransform.position,
        attackRadius, enemyLayer);
      var nearestEnemy = colliders
        .Select(c => c.GetComponent<Enemy>())
        .Where(e => e)
        .OrderBy(e =>
          Vector2.Distance(playerTransform.position, e.transform.position))
        .FirstOrDefault();

      if (nearestEnemy)
      {
        var direction = (nearestEnemy.transform.position - transform.position)
          .normalized;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Shoot();
      }
    }

    protected override void Shoot()
    {
      Instantiate(projectilePrefab, firePoint.position, transform.rotation);
    }

    private void OnDrawGizmosSelected()
    {
      Gizmos.color = Color.red;
      if (playerTransform != null)
        Gizmos.DrawWireSphere(playerTransform.position, attackRadius);
      else
        Gizmos.DrawWireSphere(transform.position, attackRadius);

      if (playerTransform != null)
      {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(playerTransform.position, orbitDistance);
      }
    }
  }
}