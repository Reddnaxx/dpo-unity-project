using _00_Scripts.Game.Entity;
using UniRx;
using UnityEngine;

namespace _00_Scripts.Game.Enemies
{
    public class Enemy : Character
    {
        [SerializeField] private float _moveSpeed = 2f;
        private Transform _target;
        private Rigidbody2D _rb;

        public void Initialize(Transform target)
        {
            _target = target;
            _rb = GetComponent<Rigidbody2D>();
        }

        protected override void Update()
        {
            base.Update();

            if (_target != null)
            {
                MoveTowardsTarget();
            }
        }

        private void MoveTowardsTarget()
        {
            Vector2 direction = (_target.position - transform.position).normalized;
            _rb.velocity = direction * _moveSpeed;
        }

        protected override void Start()
        {
            base.Start();

            OnDeath
              .Subscribe(_ => Die())
              .AddTo(this);
        }

        private void Die()
        {
            Debug.Log("Enemy has died");
            Destroy(gameObject);
        }
    }
}