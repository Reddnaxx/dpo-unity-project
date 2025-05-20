using System;
using System.Collections.Generic;

using _00_Scripts.Events;
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

  [RequireComponent(typeof(AudioSource))]
  public class GameEntryPoint : SceneEntryPoint
  {
    [Header("Result Screens")] [SerializeField]
    private GameObject gameOverUI;

    [SerializeField] private GameObject gameWinUI;

    [Header("Menus")] [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject approveToMainMenuUI;

    [Header("Weapons")] [SerializeField] private List<SelectedWeapon> weaponPrefabs;

    [Header("Audio")] [SerializeField] private AudioClip gameOverMusic;
    [SerializeField] private AudioClip gameWinMusic;

    private AudioSource _audioSource;
    private GameObject _currentMenuGameObject;
    private bool _isGameOver;
    private bool _isWin;
    private PlayerInput _playerInput;
    private WeaponType _selectedWeaponType;
    private UIRoot _uiRoot;

    private bool IsMenuOpen => _currentMenuGameObject != null;

    public override void Init()
    {
      base.Init();

      _audioSource = GetComponent<AudioSource>();
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

      EventBus.On<PlayerDeathEvent>()
        .Where(_ => !_isGameOver)
        .Subscribe(_ => OnGameOver())
        .AddTo(this);

      EventBus.On<WavesEndEvent>()
        .Where(_ => !_isWin)
        .Subscribe(_ => OnGameWin())
        .AddTo(this);
    }

    private void OnGameOver()
    {
      _isGameOver = true;

      _uiRoot.ClearScreens();
      _uiRoot.AddScreen(gameOverUI);
      _playerInput.DeactivateInput();

      _audioSource.clip = gameOverMusic;
      _audioSource.loop = false;
      _audioSource.Play();
    }

    private void OnGameWin()
    {
      _isWin = true;

      _uiRoot.ClearScreens();
      _uiRoot.AddScreen(gameWinUI);
      _playerInput.DeactivateInput();

      _audioSource.clip = gameWinMusic;
      _audioSource.loop = false;
      _audioSource.Play();
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
