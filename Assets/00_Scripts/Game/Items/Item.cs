using System.Collections.Generic;

using JetBrains.Annotations;

using UnityEngine;

namespace _00_Scripts.Game.Items
{
  [CreateAssetMenu(fileName = "Item", menuName = "Game/Items/Item")]
  public class Item : ScriptableObject
  {
    public string itemName;
    [CanBeNull] public Sprite icon;
    public List<Upgrade> upgrades;
  }
}
