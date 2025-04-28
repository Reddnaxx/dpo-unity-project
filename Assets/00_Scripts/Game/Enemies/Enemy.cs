using _00_Scripts.Game.Entity;
using UniRx;
using UnityEngine;

namespace _00_Scripts.Game.Enemies
{
    public class Enemy : Character
    {
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
        }
    }
}