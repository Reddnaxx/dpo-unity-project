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
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private bool _isActive = true;
    private GameObject _player;

    protected override void Start()
    {
            base.Start();

            OnDeath
              .Subscribe(_ => Die())
              .AddTo(this);
    }
    private void Update()
    {
      if (!_isActive || !_player) return;

      Vector2 direction = (_player.transform.position - transform.position).normalized;
      transform.position += (Vector3)(direction * (CurrentStats.Speed * Time.deltaTime));

      _spriteRenderer.flipX = direction.x < 0;
    }

    private void Die()
    {
      Debug.Log("Enemy has died");

      EventBus.Publish(new EnemyDeathEvent(100f));

      _isActive = false;
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
