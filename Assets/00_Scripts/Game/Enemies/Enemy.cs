using _00_Scripts.Events;
using _00_Scripts.Game.Entity;
using _00_Scripts.Game.Player;
using _00_Scripts.Helpers;

using UniRx;

using UnityEngine;

namespace _00_Scripts.Game.Enemies
{
  [RequireComponent(typeof(Rigidbody2D))]
  [RequireComponent(typeof(Collider2D))] // Добавляем обязательный коллайдер
  public class Enemy : Character
  {
    [Header("Combat Settings")]
    [SerializeField] private int experienceGain = 100;
    [SerializeField] private float attackRange = 0.5f;

    private Rigidbody2D _rb;
    private bool _isActive = true;
    private Transform _playerTransform;
    private float _lastAttackTime;
    private Collider2D _collider;

    protected override void Start()
    {
      base.Start();
      _rb = GetComponent<Rigidbody2D>();
      _collider = GetComponent<Collider2D>();
      _playerTransform = FindFirstObjectByType<PlayerCharacter>().transform;

      OnDeath
        .Subscribe(_ => Die())
        .AddTo(this);
    }

    private void FixedUpdate()
    {
      if (!_isActive || !_playerTransform) return;

      // Проверяем расстояние до игрока
      float distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);

      if (distanceToPlayer <= attackRange)
      {
        // Если в радиусе атаки - атакуем
        _rb.linearVelocity = Vector2.zero;
        TryAttackPlayer();
      }
      else
      {
        // Иначе - двигаемся к игроку
        Vector2 direction = (_playerTransform.position - transform.position).normalized;
        _rb.linearVelocity = direction * CurrentStats.Speed;
        spriteRenderer.flipX = direction.x < 0;
      }
    }

    private void TryAttackPlayer()
    {
      if (Time.time - _lastAttackTime >= CurrentStats.AttackSpeed)
      {
        _lastAttackTime = Time.time;
        AttackPlayer();
      }
    }

    private void AttackPlayer()
    {
      // Проверяем, все еще ли игрок в радиусе атаки
      if (Vector2.Distance(transform.position, _playerTransform.position) <= attackRange)
      {
        var player = _playerTransform.GetComponent<PlayerCharacter>();
        if (player != null)
        {
          player.TakeDamage(CurrentStats.Attack);
        }
      }
    }

    private void Die()
    {
      Debug.Log("Enemy has died");
      EventBus.Publish(new EnemyDeathEvent(experienceGain));
      _isActive = false;
      _rb.linearVelocity = Vector2.zero;
      _collider.enabled = false; // Отключаем коллайдер при смерти
      gameObject.SetActive(false);
    }

    public void Activate(Vector3 spawnPosition)
    {
      transform.position = spawnPosition;
      _isActive = true;
      _collider.enabled = true; // Включаем коллайдер при активации
      gameObject.SetActive(true);
    }

    // Обработчик столкновений (альтернативный вариант)
    private void OnCollisionStay2D(Collision2D collision)
    {
      if (!_isActive) return;

      var player = collision.collider.GetComponent<PlayerCharacter>();
      if (player != null)
      {
        TryAttackPlayer();
      }
    }
  }
}
