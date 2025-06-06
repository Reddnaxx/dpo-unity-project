using System;

using _00_Scripts.Game.Weapon.Projectiles;

using UnityEngine;

namespace _00_Scripts.Game.Weapon.Core
{
  [CreateAssetMenu(menuName = "Game/Weapon/WeaponData")]
  public class WeaponData : ScriptableObject, ICloneable
  {
    [Header("General")] public FireStrategy fireStrategy;
    public float fireRate = 0.5f;
    public float orderChangeAngle = 45f;
    public float followTime = 0.2f;

    [Header("Projectile")] public Projectile projectilePrefab;
    public float projectileSpeed = 10f;
    public float damage = 1;
    
    public object Clone() => MemberwiseClone();
  }
}
