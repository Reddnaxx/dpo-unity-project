using System;
using System.Collections.Generic;

using _00_Scripts.Events;
using _00_Scripts.Game.Entity;
using _00_Scripts.Game.Items;
using _00_Scripts.Helpers;

using UniRx;

using UnityEngine;

namespace _00_Scripts.Game.Player
{
  public class Player : Character
  {
    [SerializeField] private PlayerLevel playerLevel;

    private Inventory _inventory;

    protected override void Awake()
    {
      base.Awake();

      playerLevel = new PlayerLevel();
      _inventory = new Inventory(defaultStats);

      playerLevel.CurrentExperience
        .Subscribe(_ => OnExperienceChanged(
          playerLevel.CurrentExperience.Value,
          playerLevel.ExperienceToNextLevel,
          playerLevel.Level.Value)
        )
        .AddTo(this);

      playerLevel.Level
        .Subscribe(OnLevelUp)
        .AddTo(this);
    }

    protected override void Start()
    {
      base.Start();

      playerLevel.AddExperience(1000);
      TakeDamage(10);
    }

    public override void TakeDamage(float damage)
    {
      base.TakeDamage(damage);

      EventBus.Publish(new PlayerHPChangeEvent(CurrentStats.Health, CurrentStats.MaxHealth));
    }

    private void OnExperienceChanged(float experience, float nextExperience, int level)
    {
      EventBus.Publish(new PlayerExpChangeEvent(level, experience, nextExperience));
    }

    private void OnLevelUp(int level)
    {
      Debug.Log($"Player leveled up to level {level}");
    }
  }
}
