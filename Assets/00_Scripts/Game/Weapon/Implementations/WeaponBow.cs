using System;
using System.Collections.Generic;

using _00_Scripts.Game.Weapon.Projectiles;
using _00_Scripts.Helpers;

using DG.Tweening;

using UniRx;

using UnityEngine;

namespace _00_Scripts.Game.Weapon.Implementations
{
  public class WeaponBow : Core.Weapon
  {
    [Header("Bow Settings")] [SerializeField]
    private float maxChargeTime = 2f;

    [SerializeField] private float chargeRatioThreshold = 0.1f;
    [SerializeField] private float releaseTime = 0.5f;

    [SerializeField] private AnimationCurve damageMultiplierCurve =
      AnimationCurve.EaseInOut(0, 0, 1, 2);

    [SerializeField] private AnimationCurve velocityMultiplierCurve =
      AnimationCurve.EaseInOut(0, 0, 1, 2);

    [Header("Arrow Visuals")] [SerializeField]
    private SpriteRenderer arrowBody;

    [SerializeField] private float arrowChargeOffset = 0.5f;
    [SerializeField] private float arrowChargeStep = 4f;
    [SerializeField] private List<Sprite> bowChargeSprites;

    private Vector3 _arrowDefaultLocalPosition;
    private bool _isCharging;
    private float _chargeTime;
    private IDisposable _chargeSubscription;
    private CompositeDisposable _bowDisp;

    protected override void Awake()
    {
      base.Awake();
      _bowDisp = new CompositeDisposable();
      if (arrowBody != null)
        _arrowDefaultLocalPosition = arrowBody.transform.localPosition;
    }

    protected override void OnEnable()
    {
      base.OnEnable();

      // Начало заряда – при первом нажатии
      AttackAction
        .OnPerformedAsObservable()
        .Where(_ => !_isCharging)
        .Subscribe(_ => StartCharging())
        .AddTo(_bowDisp);

      // Отпускание – выстрел и остановка заряда
      AttackAction
        .OnCanceledAsObservable()
        .Where(_ => _isCharging)
        .Subscribe(_ =>
        {
          ReleaseAndShoot();
          StopCharging();
        })
        .AddTo(_bowDisp);
    }

    protected override void OnDisable()
    {
      _bowDisp.Clear();
      base.OnDisable();
    }

    // Отменяем автоматический fire из базового Weapon
    protected override void DoFire()
    {
    }

    private void StartCharging()
    {
      _isCharging = true;
      _chargeTime = 0f;

      _chargeSubscription = Observable.EveryUpdate()
        .Subscribe(_ =>
        {
          _chargeTime = Mathf.Min(_chargeTime + Time.deltaTime, maxChargeTime);

          // Меняем спрайт лука в зависимости от уровня заряда
          int idx = Mathf.FloorToInt(
            _chargeTime / maxChargeTime * (bowChargeSprites.Count - 1)
          );
          bodyRenderer.sprite = bowChargeSprites[idx];

          // Смещаем стрелу у тетивы
          float raw = Mathf.Lerp(0f, arrowChargeOffset, _chargeTime / maxChargeTime);
          float offset = Mathf.Round(raw * arrowChargeStep) / arrowChargeStep;
          arrowBody.transform.localPosition =
            _arrowDefaultLocalPosition - new Vector3(offset, 0, 0);
        })
        .AddTo(_bowDisp);
    }

    private void StopCharging()
    {
      _isCharging = false;
      _chargeSubscription?.Dispose();

      // Плавно возвращаем спрайт лука к первоначальному
      DOTween.To(
          () => bowChargeSprites.IndexOf(bodyRenderer.sprite),
          x => bodyRenderer.sprite = bowChargeSprites[Mathf.RoundToInt(x)],
          0, releaseTime
        )
        .SetUpdate(true);

      // Возвращаем позицию стрелы
      arrowBody.transform
        .DOLocalMove(_arrowDefaultLocalPosition, releaseTime)
        .SetUpdate(true);
    }

    private void ReleaseAndShoot()
    {
      float ratio = Mathf.Clamp01(_chargeTime / maxChargeTime);
      if (ratio < chargeRatioThreshold) return;

      // Инстанцируем снаряд и устанавливаем параметры
      var projGO = Instantiate(data.projectilePrefab, firePoint.position, transform.rotation);
      var proj = projGO.GetComponent<Projectile>();
      proj.Init(
        data.projectileSpeed * velocityMultiplierCurve.Evaluate(ratio),
        data.damage * damageMultiplierCurve.Evaluate(ratio)
      );

      // Визуальный эффект и звук выстрела
      if (shootSound)
        AudioSource.PlayOneShot(shootSound);
    }
  }
}
