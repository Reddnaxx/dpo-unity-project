using System;

using _00_Scripts.Helpers;

using DG.Tweening;

using UniRx;

using UnityEngine;
using UnityEngine.InputSystem;

namespace _00_Scripts.Game.Weapon.Core
{
  [RequireComponent(typeof(AudioSource))]
  public abstract class Weapon : MonoBehaviour
  {
    [SerializeField] protected WeaponData data;

    [Header("References")] [SerializeField]
    protected Transform firePoint;

    [SerializeField] protected AudioClip shootSound;
    [SerializeField] protected SpriteRenderer bodyRenderer;

    protected InputAction AttackAction;
    private CompositeDisposable _disposables;
    protected AudioSource AudioSource;

    protected virtual void Awake()
    {
      AudioSource = GetComponent<AudioSource>();
      _disposables = new CompositeDisposable();

      var pi = FindFirstObjectByType<PlayerInput>();
      AttackAction = pi.actions["Attack"];
    }

    protected virtual void OnEnable()
    {
      AttackAction
        .OnPerformedAsObservable()
        .ThrottleFirst(TimeSpan.FromSeconds(data.fireRate))
        .Subscribe(_ => DoFire())
        .AddTo(_disposables);

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
      var inFront = angle > 90 - data.orderChangeAngle && angle < 90 + data.orderChangeAngle;
      bodyRenderer.sortingOrder = inFront ? 0 : 1;
    }

    /// <summary>
    /// Стреляет: создаёт снаряд(ы), воспроизводит звук.
    /// </summary>
    protected virtual void DoFire()
    {
      data.fireStrategy.Fire(firePoint.position, firePoint.rotation, data);

      if (shootSound)
      {
        AudioSource.PlayOneShot(shootSound);
      }
    }

    protected virtual void OnDestroy()
    {
      _disposables.Dispose();
    }
  }
}
