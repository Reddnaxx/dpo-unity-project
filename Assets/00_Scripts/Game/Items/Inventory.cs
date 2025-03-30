using System;
using System.Collections.Generic;
using _00_Scripts.Game.Entity;
using UniRx;

namespace _00_Scripts.Game.Items
{
  [Serializable]
  public class Inventory
  {
    private readonly IStats _defaultStats;

    public IReadOnlyReactiveCollection<Item> Items => _items;
    private ReactiveCollection<Item> _items = new();

    public Inventory(IStats defaultStats)
    {
      _defaultStats = defaultStats;
    }

    public void AddItem(Item item)
    {
      _items.Add(item);
    }

    public void RemoveItem(Item item)
    {
      _items.Remove(item);
    }

    public Stats GetFinalStats()
    {
      var finalStats = new Stats(_defaultStats);

      foreach (var item in _items)
      {
        finalStats.ApplyItem(item);
      }

      return finalStats;
    }
  }
}