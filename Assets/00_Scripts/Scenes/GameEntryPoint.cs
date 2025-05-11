using _00_Scripts.Helpers;
using _00_Scripts.UI;

using UniRx;

using UnityEngine;
using UnityEngine.InputSystem;

namespace _00_Scripts.Scenes
{
  public class GameEntryPoint : SceneEntryPoint
  {
    [SerializeField] private GameObject inventoryUI;

    private PlayerInput _playerInput;
    private UIRoot _uiRoot;

    private bool IsMenuOpen => _currentMenuGameObject != null;
    private GameObject _currentMenuGameObject;

    public override void Init()
    {
      base.Init();

      _uiRoot = FindFirstObjectByType<UIRoot>();
      _playerInput = FindFirstObjectByType<PlayerInput>();

      _playerInput.actions["ToggleMenu"]
        .OnPerformedAsObservable()
        .Subscribe(_ => ShowMenu())
        .AddTo(this);
    }

    private void ShowMenu()
    {
      if (IsMenuOpen)
      {
        _uiRoot.RemoveScreen(_currentMenuGameObject);
        _playerInput.ActivateInput();
        Time.timeScale = 1f;
        return;
      }

      _currentMenuGameObject = _uiRoot.AddScreen(inventoryUI);
      _playerInput.DeactivateInput();
      Time.timeScale = 0f;
    }
  }
}
