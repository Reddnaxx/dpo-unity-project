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
    [SerializeField] protected DefaultStats defaultStats;
    [SerializeField] [CanBeNull] protected Image healthBarFill;

    [Header("Hit Sound")] [SerializeField] private AudioClip hitSound;

    private AudioSource _audioSource;

    private ReactiveProperty<float> _healthPercentage;

    protected Stats CurrentStats;

    protected float CurrentHealthPercentage =>
      CurrentStats.Health / CurrentStats.MaxHealth;

    public IObservable<Unit> OnDeath => _healthPercentage
      .Where(value => value <= 0f)
      .First()
      .AsUnitObservable();

    protected virtual void Awake()
    {
      CurrentStats = new Stats(defaultStats);

      _healthPercentage = new ReactiveProperty<float>(CurrentHealthPercentage);

      _audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Start()
    {
      if (!healthBarFill) return;

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

    public virtual void TakeDamage(float damage)
    {
      CurrentStats.TakeDamage(damage);

      _healthPercentage.Value = Mathf.Round(CurrentHealthPercentage * 100) / 100;

      if (hitSound != null && _audioSource != null)
        _audioSource.PlayOneShot(hitSound);
    }
  }
}
