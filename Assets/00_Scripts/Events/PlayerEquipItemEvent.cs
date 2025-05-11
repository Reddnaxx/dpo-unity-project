using _00_Scripts.Game.Items;

namespace _00_Scripts.Events
{
  public class PlayerEquipItemEvent
  {
    public Item NewItem { get; }
    
    public PlayerEquipItemEvent(Item newItem)
    {
      NewItem = newItem;
    }
  }
}
