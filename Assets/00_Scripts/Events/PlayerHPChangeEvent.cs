namespace _00_Scripts.Events
{
  public class PlayerHpChangeEvent
  {
    public float CurrentHealth
    {
      get;
    }

    public float MaxHealth
    {
      get;
    }

    public PlayerHpChangeEvent(float currentHealth, float maxHealth)
    {
      CurrentHealth = currentHealth;
      MaxHealth = maxHealth;
    }
  }
}
