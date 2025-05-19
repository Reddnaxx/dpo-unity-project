using _00_Scripts.Events;
using _00_Scripts.Game.Entity;
using _00_Scripts.Game.Player;
using _00_Scripts.Helpers;

using UniRx;

using UnityEngine;

namespace _00_Scripts.Game.Enemies
{
  [RequireComponent(typeof(Rigidbody2D))] // Добавляем обязательный компонент
  public class Enemy : Character
  {
    [Header("Movement Settings")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _moveSpeed = 3f;

    private Rigidbody2D _rb;
    private bool _isActive = true;
    private Transform _playerTransform;

    protected override void Start()
    {
      base.Start();
      _rb = GetComponent<Rigidbody2D>();
      _playerTransform = FindObjectOfType<PlayerCharacter>().transform;

      OnDeath
          .Subscribe(_ => Die())
          .AddTo(this);
    }

    private void FixedUpdate() // Используем FixedUpdate для физики
    {
      if (!_isActive || _playerTransform == null) return;

      Vector2 direction = (_playerTransform.position - transform.position).normalized;
      _rb.linearVelocity = direction * _moveSpeed; // Двигаем через velocity

      // Ориентация спрайта
      _spriteRenderer.flipX = direction.x < 0;
    }

    private void Die()
    {
      Debug.Log("Enemy has died");
      EventBus.Publish(new EnemyDeathEvent(100f));
      _isActive = false;
      _rb.linearVelocity = Vector2.zero; // Останавливаем
      gameObject.SetActive(false);
    }

    public void Activate(Vector3 spawnPosition)
    {
      transform.position = spawnPosition;
      _isActive = true;
      gameObject.SetActive(true);
    }
  }
}
