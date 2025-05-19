using System;
using System.Collections.Generic;

using _00_Scripts.Game.Weapon.Core;
using _00_Scripts.Game.Weapon.Projectiles.Modules;
using _00_Scripts.Helpers;

using UniRx;

using UnityEngine;

using DG.Tweening;

namespace _00_Scripts.Game.Weapon.Implementations
{
  public class WeaponBow : Core.Weapon
  {
    [Header("Bow Settings")]
    private BowWeaponData BowData => data as BowWeaponData;

    [Header("Arrow Visuals")]
    [SerializeField]
    private SpriteRenderer arrowBody;

    [SerializeField] private float arrowChargeOffset = 0.5f;
    [SerializeField] private float arrowChargeStep = 4f;
    [SerializeField] private List<Sprite> bowChargeSprites;

    [Header("Bow Sounds")]
    [SerializeField]
    private AudioClip bowDrawSound;

    [SerializeField] private AudioClip bowShootSound;
    [SerializeField] private AudioSource audioSource;

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

      // 1) Поток изменений TotalFireRate
      var fireRateStream = Observable.EveryUpdate()
        .Select(_ => TotalFireRate)
        .DistinctUntilChanged()
        .StartWith(TotalFireRate);

      // 2) Для каждого rate строим свой ThrottleFirst-поток, переключаясь на последний
      fireRateStream
        .Select(rate =>
          AttackAction
            .OnPerformedAsObservable()
            .Where(_ => !_isCharging)
            .ThrottleFirst(TimeSpan.FromSeconds(rate))
        )
        .Switch()
        .Subscribe(_ => StartCharging())
        .AddTo(_bowDisp);

      // 3) Отпускание — остаётся статической подпиской
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

    // Базовый DoFire отключаем
    protected override void DoFire() { }

    private void StartCharging()
    {
      _isCharging = true;
      _chargeTime = 0f;

      if (bowDrawSound && audioSource)
        audioSource.PlayOneShot(bowDrawSound);

      _chargeSubscription = Observable.EveryUpdate()
        .Subscribe(_ =>
        {
          _chargeTime = Mathf.Min(_chargeTime + Time.deltaTime, BowData.maxChargeTime);

          // Спрайт лука
          int idx = Mathf.FloorToInt(
            _chargeTime / BowData.maxChargeTime * (bowChargeSprites.Count - 1)
          );
          bodyRenderer.sprite = bowChargeSprites[idx];

          // Сдвиг стрелы
          float raw = Mathf.Lerp(0f, arrowChargeOffset, _chargeTime / BowData.maxChargeTime);
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

      DOTween.To(
          () => bowChargeSprites.IndexOf(bodyRenderer.sprite),
          x => bodyRenderer.sprite = bowChargeSprites[Mathf.RoundToInt(x)],
          0, BowData.releaseTime
        )
        .SetUpdate(true);

      arrowBody.transform
        .DOLocalMove(_arrowDefaultLocalPosition, BowData.releaseTime)
        .SetUpdate(true);
    }

    private void ReleaseAndShoot()
    {
      float ratio = Mathf.Clamp01(_chargeTime / BowData.maxChargeTime);
      if (ratio < BowData.chargeRatioThreshold) return;

      var velocityMul = BowData.velocityMultiplierCurve.Evaluate(ratio);
      var damageMul = BowData.damageMultiplierCurve.Evaluate(ratio);

      var finalData = BowData.Clone() as BowWeaponData;
      finalData.damage = TotalDamage;

      var projectiles = BowData.fireStrategy.Fire(
        firePoint.position, transform.rotation,
        finalData, velocityMul, damageMul
      );

      foreach (var proj in projectiles)
      {
        if (PlayerStats.HasHoming) proj.AddModule<HomingModule>();
        if (PlayerStats.HasBounce)
        {
          proj.AddModule<BounceModule>();
          proj.destroyOnHit = false;
        }
      }

      if (bowShootSound && audioSource)
        audioSource.PlayOneShot(bowShootSound);
    }
  }
}
