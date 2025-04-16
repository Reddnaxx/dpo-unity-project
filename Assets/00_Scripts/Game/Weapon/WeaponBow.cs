using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using _00_Scripts.Game.Weapon.Projectiles;
using DG.Tweening;

namespace _00_Scripts.Game.Weapon
{
  public class WeaponBow : Weapon
  {
    [Header("Bow Settings")]
    
    [SerializeField] private float maxChargeTime = 2f;
    [SerializeField] private float chargeRationThreshold = 0.1f;
    [SerializeField] private float releaseTime = 0.5f;

    [SerializeField] private AnimationCurve damageMultiplierCurve =
      AnimationCurve.EaseInOut(0, 0, 1, 2);

    [SerializeField] private AnimationCurve velocityMultiplierCurve =
      AnimationCurve.EaseInOut(0, 0, 1, 2);


    [SerializeField] private SpriteRenderer arrowBody;
    [SerializeField] private float arrowChargeOffset;
    [SerializeField] private float arrowChargeStep;

    [SerializeField] private List<Sprite> bowChargeSprites;

    private float _chargeTime;
    private bool _isCharging;
    private IDisposable _chargeSubscription;

    private Vector2 _arrowDefaultPosition;

    protected override void Start()
    {
      _arrowDefaultPosition = arrowBody.transform.localPosition;

      Observable.EveryUpdate()
        .Where(_ => UnityEngine.Input.GetButton("Fire1") && !_isCharging)
        .ThrottleFirst(TimeSpan.FromSeconds(fireRate))
        .Subscribe(_ => StartCharging())
        .AddTo(this);

      Observable.EveryUpdate()
        .Where(_ => UnityEngine.Input.GetButtonUp("Fire1") && _isCharging)
        .Subscribe(_ =>
        {
          Shoot();
          StopCharging();
        })
        .AddTo(this);
    }

    protected override void Update()
    {
      base.Update();

      arrowBody.flipY = body.flipY;
      arrowBody.sortingOrder = body.sortingOrder;
    }

    private void StartCharging()
    {
      _isCharging = true;
      _chargeTime = 0f;

      _chargeSubscription = Observable.EveryUpdate()
        .Subscribe(_ =>
        {
          var spriteIndex = Mathf.FloorToInt(_chargeTime / maxChargeTime *
                                             (bowChargeSprites.Count - 1));

          body.sprite = bowChargeSprites[spriteIndex];

          var arrowOffset = Mathf.Round(Mathf.Lerp(0, arrowChargeOffset,
            _chargeTime / maxChargeTime) * arrowChargeStep) / arrowChargeStep;
          arrowBody.transform.localPosition = new Vector3(
            _arrowDefaultPosition.x - arrowOffset, _arrowDefaultPosition.y, 0);

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

      DOTween.To(() => bowChargeSprites.IndexOf(body.sprite), 
        x => body.sprite = bowChargeSprites[Mathf.RoundToInt(x)], 
        0, releaseTime);
      
      arrowBody.transform.DOLocalMove(_arrowDefaultPosition, releaseTime);

      _chargeSubscription?.Dispose();
    }

    protected override void Shoot()
    {
      var chargeRatio = Mathf.Clamp01(_chargeTime / maxChargeTime);
      
      if (chargeRatio < chargeRationThreshold) return;
      
      var projectile = Instantiate(projectilePrefab, firePoint.position,
        transform.rotation);
      var projectileComponent = projectile.GetComponent<Projectile>();

      projectileComponent.SetDamageModifier(
        damageMultiplierCurve.Evaluate(chargeRatio));
      projectileComponent.SetVelocityModifier(
        velocityMultiplierCurve.Evaluate(chargeRatio));

      // if (projectileComponent != null)
      // {
      //     var damageField = typeof(Projectile).GetField("baseDamage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
      //     var velocityField = typeof(Projectile).GetField("velocity", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
      //
      //     if (damageField != null && velocityField != null)
      //     {
      //         var baseDamage = (float)damageField.GetValue(projectileComponent);
      //         var baseVelocity = (float)velocityField.GetValue(projectileComponent);
      //
      //         damageField.SetValue(projectileComponent, baseDamage * (1f + chargeRatio * (maxDamageMultiplier - 1f)));
      //         velocityField.SetValue(projectileComponent, baseVelocity * (1f + chargeRatio * (maxVelocityMultiplier - 1f)));
      //     }
      // }
    }

    private void OnDestroy()
    {
      StopCharging();
    }
  }
}