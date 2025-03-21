using System.Collections.Generic;
using _00_Scripts.Game.Items;
using UnityEngine;
using UnityEngine.Serialization;

namespace _00_Scripts.Game.Entity
{
  public class Entity : MonoBehaviour
  {
    [SerializeField] private DefaultStats defaultStats;

    private Stats _currentStats;
    private Inventory _inventory;

    private void Awake()
    {
      _inventory = new Inventory(defaultStats);
      _currentStats = new Stats(defaultStats);
    }

    public void AddItem(Item item)
    {
      _inventory.AddItem(item);

      _currentStats = _inventory.GetFinalStats();
    }
  }
}