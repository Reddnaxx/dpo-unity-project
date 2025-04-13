using System;
using System.Collections.Generic;
using _00_Scripts.Game.Entity;
using _00_Scripts.Game.Items;
using UniRx;
using UnityEngine;

namespace _00_Scripts.Game.Player
{
  public class Player : Character
  {
    [SerializeField] private PlayerLevel playerLevel;

    private Inventory _inventory;

    protected override void Awake()
    {
      base.Awake();

      playerLevel = new PlayerLevel();
      _inventory = new Inventory(defaultStats);

      playerLevel.Level
        .TakeUntilDestroy(gameObject)
        .Subscribe(OnLevelUp);
    }

    private void AddItem(Item item)
    {
      _inventory.AddItem(item);

      CurrentStats = _inventory.GetFinalStats();
    }

    protected override void Start()
    {
      base.Start();
      
      // playerLevel.AddExperience(1000);
    }

    private void OnLevelUp(int level)
    {
      Debug.Log($"Player leveled up to level {level}");
    }
  }
}