// BounceModule.cs

using System.Collections.Generic;
using System.Linq;

using _00_Scripts.Game.Entity;

using UniRx;

using UnityEngine;

namespace _00_Scripts.Game.Weapon.Projectiles.Modules
{
  [RequireComponent(typeof(Projectile))]
  public class BounceModule : MonoBehaviour, IProjectileModule
  {
    [Header("Bounce Settings")] [SerializeField]
    private int maxBounces = 3;

    [SerializeField] private float radius = 5f;

    private Projectile _proj;
    private int _bounces;
    private readonly List<Character> _charactersHit = new();

    public void Initialize(Projectile projectile)
    {
      _proj = projectile;

      var sub = _proj.OnHit
        .Where(_ => _bounces < maxBounces)
        .Subscribe(OnBounce);

      _proj.RegisterModuleSubscription(sub);
    }

    private void OnBounce(Collider2D currentCollider)
    {
      var hitPoint = currentCollider.ClosestPoint(transform.position);

      _charactersHit.Add(currentCollider.GetComponent<Character>());

      _bounces++;

      var hits = Physics2D.OverlapCircleAll(hitPoint, radius, LayerMask.GetMask("Enemy"))
        .Select(c => c.GetComponent<Character>())
        .Where(c => c && !_charactersHit.Contains(c));
      var best = hits
        .OrderBy(c => Vector2.Distance(hitPoint, c.transform.position))
        .FirstOrDefault();

      if (best)
      {
        Vector2 dir = (best.transform.position - (Vector3)hitPoint).normalized;
        var ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        _proj.transform.rotation = Quaternion.Euler(0, 0, ang);
        _proj.Rb.linearVelocity = dir * _proj.Velocity;
      }
      else
      {
        Destroy(_proj.gameObject);
      }

      if (_bounces >= maxBounces)
        Destroy(_proj.gameObject);
    }

    public void Dispose()
    {
    }
  }
}
