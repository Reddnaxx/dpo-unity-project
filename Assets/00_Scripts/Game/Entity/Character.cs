using System;

using DG.Tweening;

using JetBrains.Annotations;

using UniRx;

using UnityEngine;
using UnityEngine.UI;

namespace _00_Scripts.Game.Entity
{
  [RequireComponent(typeof(AudioSource))]
  public abstract class Character : MonoBehaviour
  {
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected DefaultStats defaultStats;
    [SerializeField] [CanBeNull] protected Image healthBarFill;
    [SerializeField] private float hitFlashDuration = 0.15f;

    [Header("Hit Sound")] [SerializeField] private AudioClip hitSound;

    private AudioSource _audioSource;

    protected ReactiveProperty<float> Health;

    protected Stats CurrentStats;

    protected float CurrentHealthPercentage = 1f;

    public IObservable<Unit> OnDeath => Health
      .Where(value => value <= 0f)
      .First()
      .AsUnitObservable();

    protected virtual void Awake()
    {
      CurrentStats = new Stats(defaultStats);

      Health = new ReactiveProperty<float>(CurrentStats.MaxHealth);

      _audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Start()
    {
      if (!healthBarFill) return;

      Health.Subscribe(value =>
        {
          DOTween.To(
            () => CurrentHealthPercentage,
            x =>
            {
              CurrentHealthPercentage = x;
              healthBarFill.fillAmount = CurrentHealthPercentage;
            },
            value / CurrentStats.MaxHealth,
            0.2f);
        })
        .AddTo(this);
    }

    public virtual void TakeDamage(float damage, DamageType damageType = DamageType.Physical)
    {
      var resistance = damageType switch
      {
        DamageType.Fire => CurrentStats.FireResistance,
        DamageType.Ice => CurrentStats.IceResistance,
        DamageType.Poison => CurrentStats.PoisonResistance,
        _ => 0
      };

      var finalDamage = Mathf.RoundToInt(damage * (1 - resistance));

      Health.Value = Mathf.Clamp(
        Health.Value - Mathf.Max(finalDamage - CurrentStats.PhysicalResistance, 0),
        0,
        CurrentStats.MaxHealth
      );

      spriteRenderer?
        .DOColor(Color.red, hitFlashDuration)
        .OnComplete(() => spriteRenderer.DOColor(Color.white, hitFlashDuration));

      if (hitSound != null && _audioSource != null)
        _audioSource.PlayOneShot(hitSound);
    }
  }
}
