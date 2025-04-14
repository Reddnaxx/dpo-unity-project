using System.Linq;
using System;
using UniRx;
using UnityEngine;
using _00_Scripts.Game.Weapon.Projectiles;

namespace _00_Scripts.Game.Weapon
{
    public class WeaponMagicBook : Weapon
    {
        [SerializeField] private float attackRadius = 5f;
        [SerializeField] private float orbitDistance = 1.5f;
        [SerializeField] private float orbitSpeed = 90f;
        [SerializeField] private LayerMask enemyLayer;

        private Transform _playerTransform;
        private float _currentAngle;

        protected override void Start()
        {
            _playerTransform = transform.parent;
            if (_playerTransform == null)
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
            Vector2 offset = new Vector2(Mathf.Cos(_currentAngle * Mathf.Deg2Rad), Mathf.Sin(_currentAngle * Mathf.Deg2Rad)) * orbitDistance;
            transform.position = (Vector2)_playerTransform.position + offset;

            var angle = _currentAngle;
            body.flipY = Mathf.Abs(angle) > 90;
            body.sortingOrder = angle > 45 && angle < 135 ? 0 : 1;
        }

        private void TryShootAtNearestEnemy()
        {
            var colliders = Physics2D.OverlapCircleAll(_playerTransform.position, attackRadius, LayerMask.GetMask("Default"));
            var nearestEnemy = colliders
                .Select(c => c.GetComponent<_00_Scripts.Game.Enemies.Enemy>())
                .Where(e => e != null)
                .OrderBy(e => Vector2.Distance(_playerTransform.position, e.transform.position))
                .FirstOrDefault();

            if (nearestEnemy != null)
            {
                var direction = (nearestEnemy.transform.position - transform.position).normalized;
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
            if (_playerTransform != null)
                Gizmos.DrawWireSphere(_playerTransform.position, attackRadius);
            else
                Gizmos.DrawWireSphere(transform.position, attackRadius);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, orbitDistance);
        }
    }
}