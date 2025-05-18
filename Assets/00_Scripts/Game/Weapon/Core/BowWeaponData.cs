using System;

using UnityEngine;

namespace _00_Scripts.Game.Weapon.Core
{
  [CreateAssetMenu(menuName = "Game/Weapon/BowWeaponData")]
  public class BowWeaponData : WeaponData, ICloneable
  {
    [Header("Main")] public float maxChargeTime = 2f;
    public float chargeRatioThreshold = 0.1f;
    public float releaseTime = 0.5f;

    [Header("Curves")] public AnimationCurve damageMultiplierCurve =
      AnimationCurve.EaseInOut(0, 0, 1, 2);

    public AnimationCurve velocityMultiplierCurve =
      AnimationCurve.EaseInOut(0, 0, 1, 2);

    public object Clone() => MemberwiseClone();
  }
}
