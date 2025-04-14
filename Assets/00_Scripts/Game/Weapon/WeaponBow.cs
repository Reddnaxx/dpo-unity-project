using System;
using UniRx;
using UnityEngine;
using _00_Scripts.Game.Weapon.Projectiles;

namespace _00_Scripts.Game.Weapon
{
    public class WeaponBow : Weapon
    {
        [SerializeField] private float maxChargeTime = 2f;
        [SerializeField] private float maxDamageMultiplier = 3f;
        [SerializeField] private float maxVelocityMultiplier = 2f;

        private float _chargeTime;
        private bool _isCharging;
        private IDisposable _chargeSubscription;

        protected override void Start()
        {
        }

        protected override void Update()
        {
            base.Update();

            if (UnityEngine.Input.GetButtonDown("Fire1"))
            {
                StartCharging();
            }
            else if (UnityEngine.Input.GetButtonUp("Fire1") && _isCharging)
            {
                Shoot();
                StopCharging();
            }
        }

        private void StartCharging()
        {
            _isCharging = true;
            _chargeTime = 0f;

            _chargeSubscription = Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    _chargeTime += Time.deltaTime;
                    if (_chargeTime >= maxChargeTime)
                    {
                        _chargeTime = maxChargeTime;
                    }
                })
                .AddTo(this);
        }

        private void StopCharging()
        {
            _isCharging = false;
            _chargeSubscription?.Dispose();
        }

        protected override void Shoot()
        {
            float chargeRatio = Mathf.Clamp01(_chargeTime / maxChargeTime);
            var projectile = Instantiate(projectilePrefab, firePoint.position, transform.rotation);
            var projectileComponent = projectile.GetComponent<Projectile>();

            if (projectileComponent != null)
            {
                var damageField = typeof(Projectile).GetField("baseDamage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var velocityField = typeof(Projectile).GetField("velocity", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                if (damageField != null && velocityField != null)
                {
                    float baseDamage = (float)damageField.GetValue(projectileComponent);
                    float baseVelocity = (float)velocityField.GetValue(projectileComponent);

                    damageField.SetValue(projectileComponent, baseDamage * (1f + chargeRatio * (maxDamageMultiplier - 1f)));
                    velocityField.SetValue(projectileComponent, baseVelocity * (1f + chargeRatio * (maxVelocityMultiplier - 1f)));
                }
            }
        }

        private void OnDestroy()
        {
            StopCharging();
        }
    }
}