using System;

using DG.Tweening;

using JetBrains.Annotations;

using UniRx;

using UnityEngine;
using UnityEngine.UI;

namespace _00_Scripts.Game.Entity
{
  public abstract class Character : MonoBehaviour
  {
    [SerializeField] protected DefaultStats defaultStats;
    [SerializeField] [CanBeNull] protected Image healthBarFill;

    [Header("Hit Sound")]
    [SerializeField] private AudioClip hitSound;
    private AudioSource audioSource;

    protected Stats CurrentStats;

    protected float CurrentHealthPercentage =>
      CurrentStats.Health / CurrentStats.MaxHealth;

    public IObservable<Unit> OnDeath => _healthPercentage
      .Where(value => value <= 0f)
      .First()
      .AsUnitObservable();

    private ReactiveProperty<float> _healthPercentage;

    protected virtual void Awake()
    {
      CurrentStats = new Stats(defaultStats);

      _healthPercentage = new ReactiveProperty<float>(CurrentHealthPercentage);

      audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Start()
    {
      if (healthBarFill)
      {
        _healthPercentage.Subscribe(value =>
          {
            DOTween.To(
              () => healthBarFill.fillAmount,
              x => healthBarFill.fillAmount = x,
              value,
              0.2f);
          })
          .AddTo(this);
      }
    }

    public virtual void TakeDamage(float damage)
    {
      CurrentStats.TakeDamage(damage);

      _healthPercentage.Value = Mathf.Round(CurrentHealthPercentage * 100) / 100;

      if (hitSound != null && audioSource != null)
        audioSource.PlayOneShot(hitSound);
    }
  }
}
