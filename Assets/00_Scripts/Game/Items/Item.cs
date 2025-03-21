using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace _00_Scripts.Game.Items
{
  [Serializable]
  public class Item
  {
    public string Name { get; private set; }
    public List<Upgrade> Upgrades { get; private set; }
    [CanBeNull] public Sprite Icon { get; private set; }
    public string Description { get; private set; }

    public Item(string name, string description, [CanBeNull] Sprite icon,
      List<Upgrade> upgrades)
    {
      Name = name;
      Description = description;
      Icon = icon;
      Upgrades = upgrades;
    }
  }
}