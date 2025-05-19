using System;

using _00_Scripts.Game.Entity;
using _00_Scripts.Game.Player;
using _00_Scripts.Game.Weapon.Projectiles.Modules;
using _00_Scripts.Helpers;

using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

namespace _00_Scripts.Game.Weapon.Core
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] protected WeaponData data;
        [SerializeField] protected Transform firePoint;
        [SerializeField] protected AudioClip shootSound;
        [SerializeField] protected SpriteRenderer bodyRenderer;

        protected InputAction AttackAction;
        private CompositeDisposable _disposables;
        private SerialDisposable _attackDisposable; // для динамической подписки на огонь

        protected static IStats PlayerStats => PlayerCharacter.Stats;
        protected float TotalDamage => PlayerStats.Attack * data.damage;

        // Пересчитываемая скорострельность
        protected float TotalFireRate => data.fireRate / PlayerStats.AttackSpeed;

        protected virtual void Awake()
        {
            AttackAction = FindFirstObjectByType<PlayerInput>()
                .actions["Attack"];
            _disposables = new CompositeDisposable();
            _attackDisposable = new SerialDisposable().AddTo(_disposables);
        }

        protected virtual void OnEnable()
        {
          // 1) Слежение за изменением AttackSpeed через EveryUpdate + Select + DistinctUntilChanged
          Observable.EveryUpdate()
            .Select(_ => TotalFireRate)           // читаем текущее значение
            .DistinctUntilChanged()                         // реагируем только на реальное изменение
            .StartWith(TotalFireRate)             // сразу же запустить подписку по текущему значению
            .Subscribe(value =>
            {
              // 2) Пересоздаем подписку на стрельбу с новым интервалом
              _attackDisposable.Disposable = AttackAction
                .OnPerformedAsObservable()
                .ThrottleFirst(TimeSpan.FromSeconds(value))
                .Subscribe(_ => DoFire())
                .AddTo(_disposables);
            })
            .AddTo(_disposables);

          // Постоянный апдейт прицела
          Observable.EveryUpdate()
            .Where(_ => Time.timeScale > 0)
            .Subscribe(_ => UpdateAim())
            .AddTo(_disposables);
        }

        protected virtual void OnDisable()
        {
            _disposables.Clear();
        }

        private void UpdateAim()
        {
            Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            var dir = mouseWorld - (Vector2)transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            transform
                .DORotateQuaternion(Quaternion.Euler(0, 0, angle), data.followTime)
                .SetUpdate(true);

            bodyRenderer.flipY = Mathf.Abs(angle) > 90f;
            bool inFront = angle > 90 - data.orderChangeAngle && angle < 90 + data.orderChangeAngle;
            bodyRenderer.sortingOrder = inFront ? 0 : 1;
        }

        protected virtual void DoFire()
        {
            var finalData = data.Clone() as WeaponData;
            finalData.damage = TotalDamage;
            var projectiles = data.fireStrategy.Fire(firePoint.position, firePoint.rotation, finalData);

            foreach (var projectile in projectiles)
            {
                if (PlayerStats.HasHoming)
                    projectile.AddModule<HomingModule>();

                if (PlayerStats.HasBounce)
                {
                    projectile.AddModule<BounceModule>();
                    projectile.destroyOnHit = false;
                }
            }

            if (shootSound)
                GetComponent<AudioSource>().PlayOneShot(shootSound);
        }

        protected virtual void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}
