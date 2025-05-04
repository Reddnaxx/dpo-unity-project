using _00_Scripts.Game.Weapon.Projectiles;

using UnityEngine;

namespace _00_Scripts.Game.Weapon.Core
{
  public abstract class FireStrategy : ScriptableObject
  {
    /// <summary>
    /// Делает выстрел(ы) и возвращает массив созданных Projectile.
    /// </summary>
    public abstract Projectile[] Fire(Vector3 position, Quaternion rotation, WeaponData data, float velocityMultiplier = 1f, float damageMultiplier = 1f);
  }
}
