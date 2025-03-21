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

  [System.Serializable]
  public class Stats: IStats
  {
    [field: SerializeField] public float Health { get; private set; }
    [field: SerializeField] public float MaxHealth { get; private set; }
    [field: SerializeField] public float Attack { get; private set; }
    [field: SerializeField] public float Speed { get; private set; }

    [field: SerializeField]
    public float PhysicalResistance { get; private set; }

    [field: SerializeField] public float FireResistance { get; private set; }
    [field: SerializeField] public float IceResistance { get; private set; }
    [field: SerializeField] public float PoisonResistance { get; private set; }

    public Stats(IStats stats)
    {
      Health = stats.Health;
      MaxHealth = stats.MaxHealth;
      Attack = stats.Attack;
      PhysicalResistance = stats.PhysicalResistance;
      Speed = stats.Speed;
      FireResistance = stats.FireResistance;
      IceResistance = stats.IceResistance;
      PoisonResistance = stats.PoisonResistance;
    }

    public Stats(float health, float attack, float speed,
      float physicalResistance = 0.05f, float fireResistance = 0,
      float iceResistance = 0, float poisonResistance = 0)
    {
      Health = health;
      MaxHealth = health;
      Attack = attack;
      Speed = speed;
      PhysicalResistance = physicalResistance;
      FireResistance = fireResistance;
      IceResistance = iceResistance;
      PoisonResistance = poisonResistance;
    }

    public void ApplyItem(Item item)
    {
      var tempStats = item.Upgrades
        .Select(upgrade => ApplyUpgrade(upgrade) - this);

      var aggregatedStats = tempStats.Aggregate((a, b) => a + b);
      Health += aggregatedStats.Health;
      MaxHealth += aggregatedStats.MaxHealth;
      Attack += aggregatedStats.Attack;
      Speed += aggregatedStats.Speed;
      PhysicalResistance += aggregatedStats.PhysicalResistance;
      FireResistance += aggregatedStats.FireResistance;
      IceResistance += aggregatedStats.IceResistance;
      PoisonResistance += aggregatedStats.PoisonResistance;
    }

    private Stats ApplyUpgrade(Upgrade upgrade)
    {
      var tempStats = new Stats(this);

      switch (upgrade.Type)
      {
        case UpgradeType.Health:
          UpgradeHealth(tempStats, upgrade.Value);
          break;
        case UpgradeType.Attack:
          tempStats.Attack += upgrade.Value;
          break;
        case UpgradeType.PhysicalResistance:
          tempStats.PhysicalResistance += upgrade.Value;
          break;
        case UpgradeType.Speed:
          tempStats.Speed += upgrade.Value;
          break;
        case UpgradeType.FireResistance:
          tempStats.FireResistance += upgrade.Value;
          break;
        case UpgradeType.IceResistance:
          tempStats.IceResistance += upgrade.Value;
          break;
        case UpgradeType.PoisonResistance:
          tempStats.PoisonResistance += upgrade.Value;
          break;
        case UpgradeType.HealthMultiplier:
          UpgradeHealth(tempStats, upgrade.Value, true);
          break;
        case UpgradeType.AttackMultiplier:
          tempStats.Attack *= upgrade.Value;
          break;
        case UpgradeType.SpeedMultiplier:
          tempStats.Speed *= upgrade.Value;
          break;
        case UpgradeType.PhysicalResistanceMultiplier:
          tempStats.PhysicalResistance *= upgrade.Value;
          break;
        case UpgradeType.FireResistanceMultiplier:
          tempStats.FireResistance *= upgrade.Value;
          break;
        case UpgradeType.IceResistanceMultiplier:
          tempStats.IceResistance *= upgrade.Value;
          break;
        case UpgradeType.PoisonResistanceMultiplier:
          tempStats.PoisonResistance *= upgrade.Value;
          break;
        default:
          throw new System.ArgumentOutOfRangeException();
      }

      return tempStats;
    }

    public void TakeDamage(int damage,
      DamageType damageType = DamageType.Physical)
    {
      var resistance = damageType switch
      {
        DamageType.Fire => FireResistance,
        DamageType.Ice => IceResistance,
        DamageType.Poison => PoisonResistance,
        _ => 0
      };

      var finalDamage = Mathf.RoundToInt(damage * (1 - resistance));
      Health -= Mathf.Max(finalDamage - PhysicalResistance, 0);
      Health = Mathf.Clamp(Health, 0, MaxHealth);
    }

    private void UpgradeHealth(Stats stats, float value, bool multiply = false)
    {
      var currentPercent = stats.Health / stats.MaxHealth;

      if (multiply)
      {
        stats.MaxHealth *= value;
      }
      else
      {
        stats.MaxHealth += value;
      }

      stats.Health = stats.MaxHealth * currentPercent;
    }

    public static Stats operator +(Stats a, Stats b)
    {
      return new Stats(
        a.Health + b.Health,
        a.Attack + b.Attack,
        a.Speed + b.Speed,
        a.PhysicalResistance + b.PhysicalResistance,
        a.FireResistance + b.FireResistance,
        a.IceResistance + b.IceResistance,
        a.PoisonResistance + b.PoisonResistance
      );
    }

    public static Stats operator -(Stats a, Stats b)
    {
      return new Stats(
        a.Health - b.Health,
        a.Attack - b.Attack,
        a.Speed - b.Speed,
        a.PhysicalResistance - b.PhysicalResistance,
        a.FireResistance - b.FireResistance,
        a.IceResistance - b.IceResistance,
        a.PoisonResistance - b.PoisonResistance
      );
    }
  }
}