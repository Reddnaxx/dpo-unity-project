using UnityEngine;

namespace _00_Scripts.Game.Entity
{
  [CreateAssetMenu(
    fileName = "Default Entity Stats",
    menuName = "Game/Entity/Default Entity Stats",
    order = 0
  )]
  public class DefaultStats : ScriptableObject, IStats
  {
    [field: SerializeField]
    public float Health
    {
      get;
      private set;
    }

    [field: SerializeField]
    public float MaxHealth
    {
      get;
      private set;
    }

    [field: SerializeField]
    public float Attack
    {
      get;
      private set;
    }

    [field: SerializeField]
    public float Speed
    {
      get;
      private set;
    }

    [field: SerializeField]
    public float PhysicalResistance
    {
      get;
      private set;
    }

    [field: SerializeField]
    public float FireResistance
    {
      get;
      private set;
    }

    [field: SerializeField]
    public float IceResistance
    {
      get;
      private set;
    }

    [field: SerializeField]
    public float PoisonResistance
    {
      get;
      private set;
    }
  }
}
