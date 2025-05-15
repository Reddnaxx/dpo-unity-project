namespace _00_Scripts.Game.Entity
{
  public interface IStats
  {
    public float Health { get; }
    public float MaxHealth { get; }
    public float Attack { get; }
    public float Speed { get; }

    public float PhysicalResistance { get; }
    public float FireResistance { get; }
    public float IceResistance { get; }
    public float PoisonResistance { get; }
    
    public bool HasHoming { get; }
    public bool HasBounce { get; }
  }
}
