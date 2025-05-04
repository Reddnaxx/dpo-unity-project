using System;
using System.Linq;

using _00_Scripts.Game.Enemies;

using UniRx;

using UnityEngine;

namespace _00_Scripts.Game.Weapon.Implementations
{
  [RequireComponent(typeof(Collider2D))]
  public class WeaponMagicBook : Core.Weapon
  {
    [Header("Magic Book Settings")] [SerializeField]
    private float attackRadius = 5f;

    [SerializeField] private float orbitDistance = 1.5f;
    [SerializeField] private float orbitSpeed = 90f;
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private LayerMask enemyLayer;

    private Transform _playerTransform;
    private Vector2 _currentPosition;
    private float _currentAngle;
    private readonly CompositeDisposable _bookDisp = new();

    protected override void Awake()
    {
      base.Awake();

      // Книга должна быть дочерним объектом игрока
      _playerTransform = transform.parent;
      if (_playerTransform == null)
        Debug.LogError("WeaponMagicBook must be a child of the player!");

      _currentPosition = transform.position;
      _currentAngle = 0f;
    }

    protected override void OnEnable()
    {
      // Не вызываем base.OnEnable(), чтобы отключить ввод из Weapon

      // Автоматический выстрел каждые data.fireRate секунд
      Observable
        .Interval(TimeSpan.FromSeconds(data.fireRate))
        .Subscribe(_ => TryShootAtNearestEnemy())
        .AddTo(_bookDisp);

      // Орбита и слежка за игроком каждый кадр
      Observable
        .EveryUpdate()
        .Subscribe(_ => UpdateOrbit())
        .AddTo(_bookDisp);
    }

    protected override void OnDisable()
    {
      _bookDisp.Clear();
    }

    private void UpdateOrbit()
    {
      // Обновляем угол
      _currentAngle += orbitSpeed * Time.deltaTime;

      // Вычисляем оффсет по окружности
      var rad = _currentAngle * Mathf.Deg2Rad;
      var offset = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * orbitDistance;

      // Слегка подлетаем к позиции игрока
      _currentPosition = Vector2.Lerp(
        _currentPosition,
        _playerTransform.position,
        Time.deltaTime * followSpeed
      );

      // Устанавливаем позицию книги
      transform.position = _currentPosition + offset;
    }

    private void TryShootAtNearestEnemy()
    {
      // Ищем врагов в радиусе
      var hits = Physics2D.OverlapCircleAll(
        _playerTransform.position,
        attackRadius,
        enemyLayer
      );

      // Находим ближайшего
      var nearest = hits
        .Select(c => c.GetComponent<Enemy>())
        .Where(e => e)
        .OrderBy(e => Vector2.Distance(_playerTransform.position, e.transform.position))
        .FirstOrDefault();

      if (!nearest) return;

      // Поворачиваем книгу к цели
      var dir = (nearest.transform.position - transform.position).normalized;
      var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
      transform.rotation = Quaternion.Euler(0, 0, angle);

      // Выстрел через базовый метод (fireStrategy, muzzleFlash, звук)
      DoFire();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
      if (_playerTransform != null)
      {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_playerTransform.position, attackRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_playerTransform.position, orbitDistance);
      }
    }
#endif
  }
}
