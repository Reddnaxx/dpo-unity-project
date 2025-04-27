using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace _00_Scripts.Game.Items
{
  [Serializable]
  public class Item
  {
        public string Name { get; }
            public List<Upgrade> Upgrades { get; }
            [CanBeNull] public Sprite Icon { get; }
            public string Description { get; }

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