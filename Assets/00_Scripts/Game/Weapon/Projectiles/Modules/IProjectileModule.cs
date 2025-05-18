// IProjectileModule.cs

namespace _00_Scripts.Game.Weapon.Projectiles.Modules
{
  public interface IProjectileModule
  {
    // Вызывается сразу после Awake() базового класса
    void Initialize(Projectile projectile);

    // Позволяет модулю освободить ресурсы
    void Dispose();
  }
}
