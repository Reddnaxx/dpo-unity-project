using System;
using System.Collections.Generic;

using _00_Scripts.Game.Weapon.Core;
using _00_Scripts.Game.Weapon.Projectiles;
using _00_Scripts.Game.Weapon.Projectiles.Modules;

using UnityEngine;

namespace _00_Scripts.Game.Weapon.Strategies
{
  [CreateAssetMenu(menuName = "Game/Weapon/FireStrategy/Spread")]
  public class SpreadFireStrategy : FireStrategy
  {
    [Header("Spread Settings")] [Tooltip("Количество пеллетов за один выстрел")]
    public int pelletCount = 5;

    [Tooltip("Общий угол разброса в градусах")]
    public float spreadAngle = 30f;

    public override Projectile[] Fire(Vector3 position, Quaternion rotation, WeaponData data,
      float velocityMultiplier = 1f, float damageMultiplier = 1f)
    {
      var pellets = new Projectile[pelletCount];

      // Начальный угол: сместим каждые пеллеты равномерно в диапазоне [-spread/2; +spread/2]
      var half = spreadAngle * 0.5f;
      for (var i = 0; i < pelletCount; i++)
      {
        var angleOffset = Mathf.Lerp(-half, half, pelletCount == 1 ? 0.5f : (float)i / (pelletCount - 1));
        var pelletRot = rotation * Quaternion.Euler(0, 0, angleOffset);

        // Инстанцируем и инициализируем
        var go = Instantiate(data.projectilePrefab, position, pelletRot).gameObject;
        var proj = go.GetComponent<Projectile>();

        proj.Init(
          data.projectileSpeed * velocityMultiplier,
          data.damage * damageMultiplier
        );

        pellets[i] = proj;
      }

      return pellets;
    }
  }
}
