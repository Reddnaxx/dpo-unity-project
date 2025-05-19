using System;

using _00_Scripts.Game.Entity;

using UniRx;

using UnityEngine;

namespace _00_Scripts.Game.Weapon.Projectiles.Modules
{
  [RequireComponent(typeof(Projectile))]
  public class SplitModule : MonoBehaviour, IProjectileModule
  {
    [Header("Split Settings")]
    [SerializeField] private int splits = 2;
    [SerializeField] private float splitAngle = 45f;
    [SerializeField] private Projectile splitProjectilePrefab;

    private Projectile _proj;
    private IDisposable _hitSub;

    public void Initialize(Projectile projectile)
    {
      _proj = projectile;
      _hitSub = _proj.OnHit.Subscribe(_ => Split());
      _proj.RegisterModuleSubscription(_hitSub);
    }

    private void Split()
    {
      if (splits <= 0 || splitProjectilePrefab == null) return;

      for (int i = 0; i < splits; i++)
      {
        float angle = _proj.transform.eulerAngles.z + (-splitAngle / 2 + splitAngle * i / (splits - 1));
        Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

        var newProj = Instantiate(splitProjectilePrefab, _proj.transform.position, Quaternion.Euler(0, 0, angle));
        newProj.Init(_proj.Velocity, _proj.Damage); 
        newProj.Rb.linearVelocity = dir * _proj.Velocity; 
      }

      if (_proj.destroyOnHit)
        Destroy(_proj.gameObject);
    }

    public void Dispose()
    {
      _hitSub?.Dispose();
    }
  }
}
