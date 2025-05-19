using System;
using System.Linq;

using _00_Scripts.Game.Items;

using UnityEngine;

namespace _00_Scripts.Game.Entity
{
  public enum DamageType
  {
    Physical,
    Fire,
    Ice,
    Poison
  }

  [Serializable]
  public class Stats : IStats
  {
    [field: SerializeField] public float MaxHealth { get; private set; }
    [field: SerializeField] public float Attack { get; private set; }
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public float AttackSpeed { get; private set; }

    [field: SerializeField] public float PhysicalResistance { get; private set; }

    [field: SerializeField] public float FireResistance { get; private set; }
    [field: SerializeField] public float IceResistance { get; private set; }
    [field: SerializeField] public float PoisonResistance { get; private set; }

    [field: SerializeField] public bool HasHoming { get; private set; }
    [field: SerializeField] public bool HasBounce { get; private set; }

    public Stats(IStats stats)
    {
      MaxHealth = stats.MaxHealth;
      Attack = stats.Attack;
      AttackSpeed = stats.AttackSpeed;
      PhysicalResistance = stats.PhysicalResistance;
      Speed = stats.Speed;
      FireResistance = stats.FireResistance;
      IceResistance = stats.IceResistance;
      PoisonResistance = stats.PoisonResistance;
    }

    public Stats(float health, float attack, float speed, float attackSpeed,
      float physicalResistance = 0.05f, float fireResistance = 0, float iceResistance = 0, float poisonResistance = 0,
      bool hasHoming = false, bool hasBounce = false)
    {
      MaxHealth = health;
      Attack = attack;
      Speed = speed;
      PhysicalResistance = physicalResistance;
      FireResistance = fireResistance;
      AttackSpeed = attackSpeed;

      IceResistance = iceResistance;
      PoisonResistance = poisonResistance;
      HasHoming = hasHoming;
      HasBounce = hasBounce;
    }

    public void ApplyItem(Item item)
    {
      var tempStats = item.upgrades
        .Select(stats => ApplyUpgrade(stats) - this);

      var aggregatedStats = tempStats.Aggregate((a, b) => a + b);

      MaxHealth += aggregatedStats.MaxHealth;
      Attack += aggregatedStats.Attack;
      Speed += aggregatedStats.Speed;
      AttackSpeed += aggregatedStats.AttackSpeed;
      PhysicalResistance += aggregatedStats.PhysicalResistance;
      FireResistance += aggregatedStats.FireResistance;
      IceResistance += aggregatedStats.IceResistance;
      PoisonResistance += aggregatedStats.PoisonResistance;
      HasHoming = aggregatedStats.HasHoming;
      HasBounce = aggregatedStats.HasBounce;
    }

    private Stats ApplyUpgrade(Upgrade upgrade)
    {
      var tempStats = new Stats(this);

      if (upgrade.category == UpgradeCategory.Weapon)
      {
        switch (upgrade.weaponType)
        {
          case WeaponUpgradeType.Homing:
            tempStats.HasHoming = true;
            break;
          case WeaponUpgradeType.Bounce:
            tempStats.HasBounce = true;
            break;
          default:
            throw new ArgumentException("Weapon upgrade type not found");
        }

        return tempStats;
      }

      switch (upgrade.statType)
      {
        case StatUpgradeType.Health:
          Debug.Log($"{tempStats.MaxHealth} {upgrade.value}");
          tempStats.MaxHealth += upgrade.value;
          break;
        case StatUpgradeType.Attack:
          tempStats.Attack += upgrade.value;
          break;
        case StatUpgradeType.PhysicalResistance:
          tempStats.PhysicalResistance += upgrade.value;
          break;
        case StatUpgradeType.Speed:
          tempStats.Speed += upgrade.value;
          break;
        case StatUpgradeType.AttackSpeed:
          tempStats.AttackSpeed += upgrade.value;
          break;
        case StatUpgradeType.FireResistance:
          tempStats.FireResistance += upgrade.value;
          break;
        case StatUpgradeType.IceResistance:
          tempStats.IceResistance += upgrade.value;
          break;
        case StatUpgradeType.PoisonResistance:
          tempStats.PoisonResistance += upgrade.value;
          break;
        case StatUpgradeType.HealthMultiplier:
          tempStats.MaxHealth *= upgrade.value;
          break;
        case StatUpgradeType.AttackMultiplier:
          tempStats.Attack *= upgrade.value;
          break;
        case StatUpgradeType.SpeedMultiplier:
          tempStats.Speed *= upgrade.value;
          break;
        case StatUpgradeType.AttackSpeedMultiplier:
          tempStats.AttackSpeed *= upgrade.value;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }

      return tempStats;
    }

    public static Stats operator +(Stats a, Stats b)
    {
      return new Stats(
        a.MaxHealth + b.MaxHealth,
        a.Attack + b.Attack,
        a.Speed + b.Speed,
        a.AttackSpeed + b.AttackSpeed,
        a.PhysicalResistance + b.PhysicalResistance,
        a.FireResistance + b.FireResistance,
        a.IceResistance + b.IceResistance,
        a.PoisonResistance + b.PoisonResistance,
        a.HasHoming || b.HasHoming,
        a.HasBounce || b.HasBounce
      );
    }

    public static Stats operator -(Stats a, Stats b)
    {
      return new Stats(
        a.MaxHealth - b.MaxHealth,
        a.Attack - b.Attack,
        a.Speed - b.Speed,
        a.AttackSpeed - b.AttackSpeed,
        a.PhysicalResistance - b.PhysicalResistance,
        a.FireResistance - b.FireResistance,
        a.IceResistance - b.IceResistance,
        a.PoisonResistance - b.PoisonResistance,
        a.HasHoming || b.HasHoming,
        a.HasBounce || b.HasBounce
      );
    }
  }
}
