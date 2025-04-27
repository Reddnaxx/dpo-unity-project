using _00_Scripts.Game.Weapon.Core;
using _00_Scripts.Game.Weapon.Projectiles;

using Unity.VisualScripting;

using UnityEngine;

namespace _00_Scripts.Game.Weapon.Strategies
{
  [CreateAssetMenu(menuName = "Weapon/FireStrategy/Single")]
  public class SingleFireStrategy : FireStrategy
  {
    public override Projectile[] Fire(Vector3 position, Quaternion rotation, WeaponData data)
    {
      // Инстанцируем один снаряд
      var instantiated = Instantiate(data.projectilePrefab, position, rotation);
      var proj = instantiated.GetComponent<Projectile>();
      proj.Init(data.projectileSpeed, data.damage);
      return new[] { proj };
    }
  }
}
