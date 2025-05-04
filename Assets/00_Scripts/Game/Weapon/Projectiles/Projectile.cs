using System;

using UniRx;

using UnityEngine;

namespace _00_Scripts.Game.Weapon.Projectiles
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        // Значения инициализируются через Init()
        public float Damage { get; private set; }
        public float Velocity { get; private set; }

        // События попадания и окончания жизни
        private readonly Subject<Vector2> _onHit = new Subject<Vector2>();
        private readonly Subject<Vector2> _onLifetimeEnd = new Subject<Vector2>();

        public IObservable<Vector2> OnHit => _onHit.AsObservable();
        public IObservable<Vector2> OnLifetimeEnd => _onLifetimeEnd.First().AsObservable();

        [Header("Настройки")]
        [SerializeField] private float lifetime = 5f;
        [SerializeField] private bool destroyOnHit = true;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
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
        }

        private void Start()
        {
            // Запускаем полёт
            _rb.linearVelocity = transform.right * Velocity;
        }

        /// <summary>
        /// Инициализация снаряда из FireStrategy.
        /// </summary>
        /// <param name="velocity">Скорость полёта</param>
        /// <param name="damage">Урон при попадании</param>
        /// <param name="lifetimeOverride">Опционально задать время жизни</param>
        public void Init(float velocity, float damage, float lifetimeOverride = -1f)
        {
            Velocity = velocity;
            Damage = damage;
            if (lifetimeOverride > 0f)
                lifetime = lifetimeOverride;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Шлём позицию удара
            var contactPoint = collision.contacts[0].point;
            _onHit.OnNext(contactPoint);

            // Если хитнули Character — даём урон
            if (collision.gameObject.TryGetComponent<MonoBehaviour>(out var mb))
            {
                // Предполагаем, что Character реализует метод TakeDamage
                var character = mb as _00_Scripts.Game.Entity.Character;
                character?.TakeDamage(Damage);
            }

            if (destroyOnHit)
                Destroy(gameObject);
        }
    }
}
