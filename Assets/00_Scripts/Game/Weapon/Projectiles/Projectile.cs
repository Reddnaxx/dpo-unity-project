using System;
using System.Collections.Generic;

using _00_Scripts.Game.Entity;
using _00_Scripts.Game.Weapon.Projectiles.Modules;

using JetBrains.Annotations;

using UniRx;

using UnityEngine;

namespace _00_Scripts.Game.Weapon.Projectiles
{
  [RequireComponent(typeof(Rigidbody2D))]
  public class Projectile : MonoBehaviour
  {
    [Header("Base Settings")] [SerializeField]
    private float lifetime = 5f;

    public bool destroyOnHit = true;

    public float Damage { get; private set; }
    public float Velocity { get; private set; }

    public Rigidbody2D Rb { get; private set; }

    // Потоки событий
    private readonly Subject<Collider2D> _onHit = new();
    private readonly Subject<Vector2> _onLifetimeEnd = new();
    private readonly Subject<Unit> _onUpdate = new();

    public IObservable<Collider2D> OnHit => _onHit.AsObservable();
    public IObservable<Vector2> OnLifetimeEnd => _onLifetimeEnd.AsObservable();
    public IObservable<Unit> OnUpdate => _onUpdate.AsObservable();

    private readonly List<IDisposable> _moduleSubscriptions = new();
    private readonly List<IProjectileModule> _modules = new();

    protected virtual void Awake()
    {
      Rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
      // Таймер жизни
      Observable.Timer(TimeSpan.FromSeconds(lifetime))
        .Subscribe(_ =>
        {
          _onLifetimeEnd.OnNext(transform.position);
          Destroy(gameObject);
        })
        .AddTo(this);

      // Обновление каждый кадр
      Observable.EveryUpdate()
        .Subscribe(_ => _onUpdate.OnNext(Unit.Default))
        .AddTo(this);
    }

    private void Start()
    {
      Rb.linearVelocity = transform.right * Velocity;
    }

    public virtual void Init(float velocity, float damage, float lifetimeOverride = -1f)
    {
      Velocity = velocity;
      Damage = damage;
      if (lifetimeOverride > 0f)
        lifetime = lifetimeOverride;
    }

    public T AddModule<T>() where T : Component, IProjectileModule
    {
      // Добавляем компонент заданного типа
      var module = gameObject.AddComponent<T>();
      _modules.Add(module);
      module.Initialize(this);
      return module;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
      if (other.TryGetComponent<Character>(out var ch))
      {
        ch.TakeDamage(Damage);
        if (destroyOnHit)
        {
          Destroy(gameObject);
        }
      }
      else
      {
        Destroy(gameObject);
      }

      _onHit.OnNext(other);
    }

    private void OnDestroy()
    {
      // Чистим подписки и модули
      foreach (var sub in _moduleSubscriptions) sub.Dispose();
      foreach (var mod in _modules) mod.Dispose();
    }

    // Позволяет модулям сохранять свои подписки
    public void RegisterModuleSubscription(IDisposable disposable)
    {
      _moduleSubscriptions.Add(disposable);
    }
  }
}
