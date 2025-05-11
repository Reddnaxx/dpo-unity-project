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
    FireResistance,
    IceResistance,
    PoisonResistance,
  }

  [Serializable]
  public class Upgrade
  {
    public UpgradeType type;
    public float value;
  }
}
