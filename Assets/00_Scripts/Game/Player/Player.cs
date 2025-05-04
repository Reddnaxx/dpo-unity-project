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
        .Where(lvl => lvl > 1)
        .Subscribe(OnLevelUp)
        .AddTo(this);

      EventBus.On<EnemyDeathEvent>()
        .Subscribe(evt => playerLevel.AddExperience(evt.ExperiencePoints));
    }

    protected override void Start()
    {
      base.Start();

      playerLevel.AddExperience(0);
      TakeDamage(0);
    }

    public override void TakeDamage(float damage)
    {
      base.TakeDamage(damage);

      EventBus.Publish(new PlayerHpChangeEvent(CurrentStats.Health, CurrentStats.MaxHealth));
    }

    private void OnExperienceChanged(float experience, float nextExperience, int level)
    {
      EventBus.Publish(new PlayerExpChangeEvent(level, experience, nextExperience));
    }

    private void OnLevelUp(int level)
    {
      EventBus.Publish(new PlayerLevelUpEvent(level));
    }
  }
}
