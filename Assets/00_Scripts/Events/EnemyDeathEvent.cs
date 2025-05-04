namespace _00_Scripts.Events
{
  public class EnemyDeathEvent
  {
    public float ExperiencePoints { get; }
    
    public EnemyDeathEvent(float experiencePoints)
    {
      ExperiencePoints = experiencePoints;
    }
  }
}
