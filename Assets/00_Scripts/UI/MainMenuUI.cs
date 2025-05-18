using UnityEditor;

using UnityEngine;

namespace _00_Scripts.UI
{
  public class MainMenuUI : MonoBehaviour
  {
    [SerializeField] private WeaponSelectUI weaponSelectUI;

    private UIRoot _uiRoot;

    private void Start() => _uiRoot = FindFirstObjectByType<UIRoot>();

    public void OnPlayClick() => _uiRoot.AddScreen(weaponSelectUI.gameObject);

    public void OnExitClick()
    {
#if UNITY_EDITOR
      EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
    }
  }
}
