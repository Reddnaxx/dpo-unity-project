using UnityEngine;

namespace _00_Scripts.UI
{
  public class MainMenuUI : MonoBehaviour
  {
    [SerializeField] private WeaponSelectUI weaponSelectUI;
    [SerializeField] private SettingsUI settingsUI;

    private UIRoot _uiRoot;

    private void Start() => _uiRoot = FindFirstObjectByType<UIRoot>();

    public void OnPlayClick() => _uiRoot.AddScreen(weaponSelectUI.gameObject);

    public void OnSettingsClick() => _uiRoot.AddScreen(settingsUI.gameObject);
  }
}
