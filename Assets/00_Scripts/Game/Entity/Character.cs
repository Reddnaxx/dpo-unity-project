using _00_Scripts.Game.Items;
using UnityEngine;

namespace _00_Scripts.Game.Entity
{
  public class Character : MonoBehaviour
  {
    [SerializeField] protected DefaultStats defaultStats;

    protected Stats CurrentStats;
    protected Inventory Inventory;

    protected virtual void Awake()
    {
      Inventory = new Inventory(defaultStats);
      CurrentStats = new Stats(defaultStats);
    }

    protected virtual void AddItem(Item item)
    {
      Inventory.AddItem(item);

      CurrentStats = Inventory.GetFinalStats();
    }
  }
}