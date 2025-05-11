using System;
using System.Collections.Generic;

using _00_Scripts.Game.Entity;

using UniRx;

namespace _00_Scripts.Game.Items
{
  [Serializable]
  public class Inventory
  {
    public IReadOnlyReactiveCollection<Item> Items => _items;
    private readonly ReactiveCollection<Item> _items = new();

    public void AddItem(Item item)
    {
      _items.Add(item);
    }

    public void RemoveItem(Item item)
    {
      _items.Remove(item);
    }

    public Stats GetFinalStats(IStats currentStats)
    {
      var finalStats = new Stats(currentStats);

      foreach (var item in _items) finalStats.ApplyItem(item);

      return finalStats;
    }
  }
}
