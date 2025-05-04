using _00_Scripts.Events;
using _00_Scripts.Helpers;
using _00_Scripts.UI;

using UniRx;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace _00_Scripts.Game.Rewards
{
  public class LevelUpRewardService : MonoBehaviour
  {
    [SerializeField] private GameObject levelUpRewardUIPrefab;

    private UIRoot _uiRoot;
    private PlayerInput _playerInput;

    private void Awake()
    {
      _playerInput = FindFirstObjectByType<PlayerInput>();
      _uiRoot = FindFirstObjectByType<UIRoot>();

      EventBus.On<PlayerLevelUpEvent>()
        .Subscribe(evt => OnLevelUp(evt.CurrentLevel))
        .AddTo(this);
    }

    private void OnLevelUp(int level)
    {
      var screen = _uiRoot.AddScreen(levelUpRewardUIPrefab);
      _playerInput.DeactivateInput();

      Time.timeScale = 0f;
      
      if (screen.TryGetComponent(out LevelUpRewardsUI levelUpAwardsUI))
      {
        levelUpAwardsUI.GenerateNewRewards();
      }
      else
      {
        Debug.LogError("LevelUpAwardsUI component not found on the screen prefab.");
      }
    }
  }
}
