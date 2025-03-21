using System;

namespace _00_Scripts.Game.Items
{
  public enum UpgradeType
  {
    Health,
    HealthMultiplier,
    Attack,
    AttackMultiplier,
    Speed,
    SpeedMultiplier,
    PhysicalResistance,
    PhysicalResistanceMultiplier,
    FireResistance,
    FireResistanceMultiplier,
    IceResistance,
    IceResistanceMultiplier,
    PoisonResistance,
    PoisonResistanceMultiplier,
  }

  [Serializable]
  public class Upgrade
  {
    public UpgradeType Type { get; private set; }
    public float Value { get; private set; }

    public Upgrade(UpgradeType type, float value)
    {
      Type = type;
      Value = value;
    }
  }
}