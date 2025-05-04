using _00_Scripts.Game.Entity;
using UniRx;
using UnityEngine;

namespace _00_Scripts.Game.Enemies
{
    public class Enemy : Character
    {
        [SerializeField] private float moveSpeed = 2f;
        private GameObject player;
        private bool isActive = false;

        protected override void Start()
        {
            base.Start();

            OnDeath
                .Subscribe(_ => Die())
                .AddTo(this);

            player = GameObject.FindWithTag("Player");
        }

        private void Update()
        {
            if (!isActive || player == null) return;

            Vector2 direction = (player.transform.position - transform.position).normalized;
            transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
        }

        private void Die()
        {
            Debug.Log("Enemy has died");
            isActive = false;
            gameObject.SetActive(false);
        }

        public void Activate(Vector3 spawnPosition)
        {
            transform.position = spawnPosition;
            isActive = true;
            gameObject.SetActive(true);
        }
    }
}