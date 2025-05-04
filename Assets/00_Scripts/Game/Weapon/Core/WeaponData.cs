using _00_Scripts.Game.Weapon.Projectiles;

using UnityEngine;

namespace _00_Scripts.Game.Weapon.Core
{
  [CreateAssetMenu(menuName = "Game/Weapon/WeaponData")]
  public class WeaponData : ScriptableObject
  {
    [Header("General")]
    public FireStrategy fireStrategy;
    public float fireRate = 0.5f;
    public float orderChangeAngle = 45f;

    [Header("Projectile")]
    public Projectile projectilePrefab;
    public float projectileSpeed = 10f;
    public int damage = 1;
  }
}
