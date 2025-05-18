using _00_Scripts.Game.Weapon.Core;

namespace _00_Scripts.Events
{
  public class PlayerSelectWeaponEvent
  {
    public PlayerSelectWeaponEvent(WeaponType selectedWeapon)
    {
      SelectedWeapon = selectedWeapon;
    }

    public WeaponType SelectedWeapon { get; }
  }
}
