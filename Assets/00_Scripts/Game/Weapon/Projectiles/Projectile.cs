using System;
using _00_Scripts.Game.Entity;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace _00_Scripts.Game.Weapon.Projectiles
{
  public abstract class Projectile : MonoBehaviour
  {
    public float Damage => _damageModifier * baseDamage;
    public float Velocity => _velocityModifier * baseVelocity;
    
    [SerializeField] private float baseDamage = 10f;
    
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float baseVelocity = 10f;
    [SerializeField] private bool destroyOnHit = true;

    private float _damageModifier = 1f;
    private float _velocityModifier = 1f;

    public IObservable<Vector2> OnHit => _onHit.AsObservable();

    public IObservable<Vector2> OnLifetimeEnd =>
      _onLifetimeEnd.First().AsObservable();

    private Subject<Vector2> _onHit;
    private Subject<Vector2> _onLifetimeEnd;

    private Rigidbody2D _rb;

    private void Awake()
    {
      _onHit = new Subject<Vector2>();
      _onLifetimeEnd = new Subject<Vector2>();

      _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
      StartLifetimeTimer();

      _rb.linearVelocity = transform.right * Velocity;
    }
    
    public void SetDamageModifier(float modifier)
    {
      _damageModifier = modifier;
    }

    public void SetVelocityModifier(float modifier)
    {
      _velocityModifier = modifier;
    }

    private void StartLifetimeTimer() => Observable
      .Timer(TimeSpan.FromSeconds(lifetime))
      .Subscribe(_ =>
      {
        _onLifetimeEnd.OnNext(transform.position);
        Destroy(gameObject);
      })
      .AddTo(this);

    private void OnCollisionEnter2D(Collision2D collision)
    {
      _onHit.OnNext(collision.contacts[0].point);
      
      if (collision.gameObject.TryGetComponent<Character>(out var character))
      {
        character.TakeDamage(Damage);
      }

      if (destroyOnHit)
      {
        Destroy(gameObject);
      }
    }
  }
}