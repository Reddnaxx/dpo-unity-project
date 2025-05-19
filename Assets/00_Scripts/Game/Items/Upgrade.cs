using System;

namespace _00_Scripts.Game.Items
{
  public enum UpgradeCategory
  {
    Stat,
    Weapon
  }

  public enum StatUpgradeType
  {
    Health,
    HealthMultiplier,
    Attack,
    AttackMultiplier,
    Speed,
    SpeedMultiplier,
    AttackSpeed,
    AttackSpeedMultiplier,
    PhysicalResistance,
    FireResistance,
    IceResistance,
    PoisonResistance,
  }

  public enum WeaponUpgradeType
  {
    Homing,
    Bounce
  }

  [Serializable]
  public class Upgrade
  {
    public UpgradeCategory category;

    public StatUpgradeType statType;
    public WeaponUpgradeType weaponType;

    public float value;
  }
}
