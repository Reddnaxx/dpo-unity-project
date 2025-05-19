using System;

using _00_Scripts.Events;
using _00_Scripts.Game.Entity;
using _00_Scripts.Game.Items;
using _00_Scripts.Helpers;

using UniRx;

using UnityEngine;

namespace _00_Scripts.Game.Player
{
  public class PlayerCharacter : Character
  {
    public static Inventory Inventory { get; private set; }
    public static IStats Stats { get; private set; }

    [SerializeField] private PlayerLevel playerLevel;
    [SerializeField] private Transform weaponPivot;

    private Inventory _inventory;
    public static ReadOnlyReactiveProperty<float> HealthProperty;

    public void Init(Weapon.Core.Weapon weapon)
    {
      Instantiate(weapon, weaponPivot);
      HealthProperty = Health.ToReadOnlyReactiveProperty();
    }

    protected override void Awake()
    {
      base.Awake();

      playerLevel = new PlayerLevel();

      _inventory = new Inventory();
      Inventory = _inventory;

      Stats = CurrentStats;

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

      EventBus.On<PlayerEquipItemEvent>()
        .Subscribe(evt =>
        {
          _inventory.AddItem(evt.NewItem);
          Stats = CurrentStats = _inventory.GetFinalStats(defaultStats);
          Health.Value = CurrentHealthPercentage * CurrentStats.MaxHealth;

          EventBus.Publish(new PlayerHpChangeEvent(Health.Value, CurrentStats.MaxHealth));
        });
    }

    protected override void Start()
    {
      base.Start();

      playerLevel.AddExperience(1);
      TakeDamage(0);
    }

    public override void TakeDamage(float damage, DamageType damageType = DamageType.Physical)
    {
      base.TakeDamage(damage, damageType);

      EventBus.Publish(new PlayerHpChangeEvent(Health.Value, CurrentStats.MaxHealth));
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
