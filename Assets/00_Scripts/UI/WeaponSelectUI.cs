using _00_Scripts.Constants;
using _00_Scripts.Events;
using _00_Scripts.Game.Weapon.Core;
using _00_Scripts.Helpers;

namespace _00_Scripts.UI
{
  public class WeaponSelectUI : UIFadeScreen
  {
    private void SelectWeapon(WeaponType weaponType)
    {
      EventBus.Publish(new PlayerSelectWeaponEvent(weaponType));
      Close();

      EventBus.Publish(new LoadSceneEvent(SceneNames.Map));
    }

    public void SelectPistol() => SelectWeapon(WeaponType.Pistol);
    public void SelectShotgun() => SelectWeapon(WeaponType.Shotgun);
    public void SelectBow() => SelectWeapon(WeaponType.Bow);
    public void SelectBook() => SelectWeapon(WeaponType.Book);
  }
}
