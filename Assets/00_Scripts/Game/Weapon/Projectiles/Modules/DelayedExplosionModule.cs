using System;

using _00_Scripts.Game.Enemies;

using UniRx;

using UnityEngine;

namespace _00_Scripts.Game.Weapon.Projectiles.Modules
{
  [RequireComponent(typeof(Projectile))]
  public class DelayedExplosionModule : MonoBehaviour, IProjectileModule
  {
    [Header("Mine Settings")]
    [SerializeField] private float explosionDelay = 2f;
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private GameObject explosionEffect;

    private Projectile _proj;
    private bool _isStuck;
    private IDisposable _timerSub;

    public void Initialize(Projectile projectile)
    {
      _proj = projectile;
      _proj.OnHit.Subscribe(OnStick).AddTo(this);
      _proj.destroyOnHit = false; // Отключаем автоматическое уничтожение
    }

    private void OnStick(Collider2D collider)
    {
      if (_isStuck) return;

      _isStuck = true;
      _proj.Rb.linearVelocity = Vector2.zero;
      _proj.Rb.isKinematic = true;

      // Таймер до взрыва
      _timerSub = Observable.Timer(TimeSpan.FromSeconds(explosionDelay))
          .Subscribe(_ => Explode())
          .AddTo(this);
    }

    private void Explode()
    {
      // Визуальный эффект
      if (explosionEffect)
        Instantiate(explosionEffect, transform.position, Quaternion.identity);

      // Поиск целей в радиусе
      var hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
      foreach (var hit in hits)
      {
        if (hit.TryGetComponent<Enemy>(out var character))
        {
          character.TakeDamage(_proj.Damage);
        }
      }

      Destroy(_proj.gameObject);
    }

    public void Dispose()
    {
      _timerSub?.Dispose();
    }

    private void OnDrawGizmosSelected()
    {
      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
  }
}
