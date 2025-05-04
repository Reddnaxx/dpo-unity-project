using UnityEngine;

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
    PoisonResistanceMultiplier
  }

  [CreateAssetMenu(fileName = "Upgrade", menuName = "Game/Items/Upgrade")]
  public class Upgrade: ScriptableObject
  {
    public UpgradeType type;
    public float value;
  }
}
