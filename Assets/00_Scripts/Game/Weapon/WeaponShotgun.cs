using UnityEngine;

namespace _00_Scripts.Game.Weapon
{
  public class WeaponShotgun : Weapon
  {
    [SerializeField] private int pelletCount = 10;
    [SerializeField] private float spreadAngle = 15f;

    protected override void Shoot()
    {
      for (var i = 0; i < pelletCount; i++)
      {
        var angle = Random.Range(-spreadAngle, spreadAngle);
        var direction = Quaternion.Euler(0, 0, angle) * firePoint.right;
        
        var projectile = Instantiate(projectilePrefab, firePoint.position,
          Quaternion.identity);
        projectile.transform.right = direction;
      }
    }
  }
}