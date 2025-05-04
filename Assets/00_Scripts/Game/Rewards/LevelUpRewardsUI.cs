using System.Collections.Generic;
using System.Linq;

using _00_Scripts.Game.Items;
using _00_Scripts.UI;

using UnityEngine;
using UnityEngine.InputSystem;

namespace _00_Scripts.Game.Rewards
{
  public class LevelUpRewardsUI : MonoBehaviour
  {
    [SerializeField] private LevelUpRewardsData data;
    [SerializeField] private Transform rewardsListUI;

    [Header("References")] [SerializeField]
    private LevelUpRewardItem rewardItemPrefab;

    private List<Item> _rewards;
    private PlayerInput _playerInput;
    private UIRoot _uiRoot;

    private void Awake()
    {
      _uiRoot = FindFirstObjectByType<UIRoot>();
      _playerInput = FindFirstObjectByType<PlayerInput>();
      _rewards = new List<Item>();
    }

    public void GenerateNewRewards()
    {
      _rewards.Clear();

      _rewards.AddRange(data.rewardsPool.OrderBy(_ => Random.value).Take(3));
      UpdateUI();
    }

    public void OnSelect(Item item)
    {
      Debug.Log(item.itemName);
      Time.timeScale = 1f;

      _playerInput.ActivateInput();
      _uiRoot.RemoveScreen(gameObject);
    }

    private void UpdateUI()
    {
      for (var i = 0; i < rewardsListUI.childCount; i++)
      {
        Destroy(rewardsListUI.GetChild(i));
      }

      foreach (var reward in _rewards)
      {
        var item = Instantiate(rewardItemPrefab, rewardsListUI);

        item.Init(reward, () => OnSelect(reward));
      }
    }
  }
}
