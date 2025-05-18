// HomingModule.cs

using System;
using System.Linq;

using _00_Scripts.Game.Entity;

using UniRx;

using UnityEngine;

namespace _00_Scripts.Game.Weapon.Projectiles.Modules
{
  [RequireComponent(typeof(Projectile))]
  public class HomingModule : MonoBehaviour, IProjectileModule
  {
    [Header("Homing Settings")] [SerializeField]
    private float detectionRadius = 3f; // Радиус поиска целей

    [SerializeField] private float turnSpeed = 480f; // Градусы в секунду

    private Projectile _proj;
    private Character _currentTarget;
    private IDisposable _updateSub;

    public void Initialize(Projectile projectile)
    {
      _proj = projectile;

      // Подписываемся на каждый кадр
      _updateSub = _proj.OnUpdate
        .Subscribe(_ => UpdateHoming());

      // Регистрируем, чтобы автоматически отписаться при уничтожении
      _proj.RegisterModuleSubscription(_updateSub);
    }

    private void UpdateHoming()
    {
      // 1) Если нет текущей цели или она уничтожена — ищем новую
      if (!_currentTarget)
      {
        _currentTarget = FindNearestEnemy();
        if (!_currentTarget)
          return; // нет целей в радиусе
      }

      // 2) Вычисляем угол к цели
      Vector2 direction = (_currentTarget.transform.position - _proj.transform.position).normalized;
      var targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

      // 3) Плавно поворачиваемся к цели
      var currentAngle = _proj.transform.eulerAngles.z;
      var newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, turnSpeed * Time.deltaTime);
      _proj.transform.rotation = Quaternion.Euler(0, 0, newAngle);

      // 4) Сразу корректируем скорость по новому направлению
      var newDir = new Vector2(Mathf.Cos(newAngle * Mathf.Deg2Rad), Mathf.Sin(newAngle * Mathf.Deg2Rad));
      _proj.Rb.linearVelocity = newDir * _proj.Velocity;
    }

    private Character FindNearestEnemy()
    {
      // Находит всех врагов в радиусе
      var cols = Physics2D.OverlapCircleAll(_proj.transform.position, detectionRadius, LayerMask.GetMask("Enemy"));
      return cols
        .Select(c => c.GetComponent<Character>())
        .Where(ch => ch)
        .OrderBy(ch => Vector2.Distance(_proj.transform.position, ch.transform.position))
        .FirstOrDefault();
    }

    public void Dispose()
    {
      _updateSub?.Dispose();
    }

    private void OnDrawGizmosSelected()
    {
      // Для визуализации радиуса в сцене
      Gizmos.color = Color.cyan;
      Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
  }
}
