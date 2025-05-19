using System;

using _00_Scripts.Events;
using _00_Scripts.Game.Entity;
using _00_Scripts.Game.Player;
using _00_Scripts.Helpers;

using UniRx;

using UnityEngine;

namespace _00_Scripts.Game.Enemies
{
  public class Enemy : Character
  {
    [Header("Attack Settings")]
    [SerializeField] private float _attackDamage = 10f;
    [SerializeField] private float _attackCooldown = 1f;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    private bool _isActive = true;
    private GameObject _player;
    private bool _canAttack = true;
    private IDisposable _attackCooldownSub;

    protected override void Start()
    {
      base.Start();
      _player = GameObject.FindGameObjectWithTag("Player"); // Ищем игрока по тегу

      OnDeath
        .Subscribe(_ => Die())
        .AddTo(this);
    }

    private void Update()
    {
      if (!_isActive || !_player) return;

      // Движение к игроку
      Vector2 direction = (_player.transform.position - transform.position).normalized;
      transform.position += (Vector3)(direction * (CurrentStats.Speed * Time.deltaTime));

      // Ориентация спрайта
      _spriteRenderer.flipX = direction.x < 0;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
      if (!_canAttack || !_isActive) return;

      if (collision.gameObject.CompareTag("Player"))
      {
        var player = collision.gameObject.GetComponent<PlayerCharacter>();
        if (player != null)
        {
          player.TakeDamage(_attackDamage);
          StartAttackCooldown();
        }
      }
    }

    private void StartAttackCooldown()
    {
      _canAttack = false;
      _attackCooldownSub?.Dispose();
      _attackCooldownSub = Observable.Timer(TimeSpan.FromSeconds(_attackCooldown))
          .Subscribe(_ => _canAttack = true)
          .AddTo(this);
    }

    private void Die()
    {
      Debug.Log("Enemy has died");
      EventBus.Publish(new EnemyDeathEvent(100f));
      _isActive = false;
      _attackCooldownSub?.Dispose();
      gameObject.SetActive(false);
    }

    public void Activate(Vector3 spawnPosition)
    {
      transform.position = spawnPosition;
      _isActive = true;
      _canAttack = true;
      gameObject.SetActive(true);
    }
  }
}
