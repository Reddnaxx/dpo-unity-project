namespace _00_Scripts.Events
{
  public class PlayerLevelUpEvent
  {
    public int CurrentLevel
    {
      get;
    }

    public PlayerLevelUpEvent(int currentLevel) => CurrentLevel = currentLevel;
  }
}
