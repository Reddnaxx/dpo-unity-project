using System;
using System.Collections.Generic;

using _00_Scripts.Game.Player;
using _00_Scripts.Game.Weapon.Core;
using _00_Scripts.Helpers;
using _00_Scripts.UI;

using UniRx;

using UnityEngine;
using UnityEngine.InputSystem;

namespace _00_Scripts.Scenes
{
  [Serializable]
  internal class SelectedWeapon
  {
    [field: SerializeField] public WeaponType Type { get; private set; }
    [field: SerializeField] public Weapon Weapon { get; private set; }

    public SelectedWeapon(WeaponType type, Weapon weapon)
    {
      Type = type;
      Weapon = weapon;
    }
  }

  public class GameEntryPoint : SceneEntryPoint
  {
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject approveToMainMenuUI;

    [SerializeField] private List<SelectedWeapon> weaponPrefabs;
    private GameObject _currentMenuGameObject;

    private PlayerInput _playerInput;
    private WeaponType _selectedWeaponType;
    private UIRoot _uiRoot;

    private bool IsMenuOpen => _currentMenuGameObject != null;

    public override void Init()
    {
      base.Init();

      _uiRoot = FindFirstObjectByType<UIRoot>();
      _playerInput = FindFirstObjectByType<PlayerInput>();

      _playerInput.actions["ToggleMenu"]
        .OnPerformedAsObservable()
        .Subscribe(_ => ShowMenu())
        .AddTo(this);

      _playerInput.actions["ToMainMenu"]
        .OnPerformedAsObservable()
        .Subscribe(_ => ShowToMainMenuApprove())
        .AddTo(this);

      var player = FindFirstObjectByType<PlayerCharacter>();

      player.Init(weaponPrefabs.Find(weapon => weapon.Type == _selectedWeaponType).Weapon ?? weaponPrefabs[0].Weapon);
    }

    public void SetWeapon(WeaponType weaponType) => _selectedWeaponType = weaponType;

    private void ShowToMainMenuApprove()
    {
      if (IsMenuOpen)
      {
        _uiRoot.RemoveScreen(_currentMenuGameObject);
        _playerInput.ActivateInput();
        Time.timeScale = 1f;
        return;
      }

      _currentMenuGameObject = _uiRoot.AddScreen(approveToMainMenuUI);
      _playerInput.DeactivateInput();
      Time.timeScale = 0f;
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
