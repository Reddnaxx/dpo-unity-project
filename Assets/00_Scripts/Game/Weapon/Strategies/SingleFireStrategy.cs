using System;
using System.Collections.Generic;

using _00_Scripts.Game.Weapon.Core;
using _00_Scripts.Game.Weapon.Projectiles;
using _00_Scripts.Game.Weapon.Projectiles.Modules;

using UnityEngine;

namespace _00_Scripts.Game.Weapon.Strategies
{
  [CreateAssetMenu(menuName = "Game/Weapon/FireStrategy/Single")]
  public class SingleFireStrategy : FireStrategy
  {
    public override Projectile[] Fire(Vector3 position, Quaternion rotation, WeaponData data,
      float velocityMultiplier = 1f, float damageMultiplier = 1f)
    {
      // Инстанцируем один снаряд
      var instantiated = Instantiate(data.projectilePrefab, position, rotation);
      var proj = instantiated.GetComponent<Projectile>();

      proj.Init(
        data.projectileSpeed * velocityMultiplier,
        data.damage * damageMultiplier
      );

      return new[] { proj };
    }
  }
}
