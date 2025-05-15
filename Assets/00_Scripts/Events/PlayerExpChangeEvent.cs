namespace _00_Scripts.Events
{
  public class PlayerExpChangeEvent
  {
    public int CurrentLevel
    {
      get;
    }

    public float CurrentExperience
    {
      get;
    }

    public float ExperienceToNextLevel
    {
      get;
    }

    public PlayerExpChangeEvent(int currentLevel, float currentExperience, float experienceToNextLevel)
    {
      CurrentLevel = currentLevel;
      CurrentExperience = currentExperience;
      ExperienceToNextLevel = experienceToNextLevel;
    }
  }
}
