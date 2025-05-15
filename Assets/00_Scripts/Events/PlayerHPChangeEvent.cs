namespace _00_Scripts.Events
{
  public class PlayerHPChangeEvent
  {
    public float CurrentHealth
    {
      get;
    }

    public float MaxHealth
    {
      get;
    }

    public PlayerHPChangeEvent(float currentHealth, float maxHealth)
    {
      CurrentHealth = currentHealth;
      MaxHealth = maxHealth;
    }
  }
}
