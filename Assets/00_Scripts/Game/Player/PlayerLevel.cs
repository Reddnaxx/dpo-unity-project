using System;

using UniRx;

using UnityEngine;

namespace _00_Scripts.Game.Player
{
  [Serializable]
  public class PlayerLevel
  {
    [field: SerializeField] public float ExperienceToNextLevelModifier { get; private set; } = 1.5f;

    public ReactiveProperty<int> Level { get; }
    public ReactiveProperty<float> CurrentExperience { get; }
    public float ExperienceToNextLevel { get; private set; } = 100;

    public PlayerLevel(int startLevel = 1)
    {
      Level = new ReactiveProperty<int>(startLevel);
      CurrentExperience = new ReactiveProperty<float>(0);
    }

    public void AddExperience(float experience)
    {
      while (true)
      {
        var nextLevelExpOld = ExperienceToNextLevel;
        CurrentExperience.Value += experience;

        if (CurrentExperience.Value < ExperienceToNextLevel) return;
        
        ExperienceToNextLevel = Mathf.RoundToInt(ExperienceToNextLevel * ExperienceToNextLevelModifier);

        Level.Value++;
        CurrentExperience.Value -= nextLevelExpOld;

        if (CurrentExperience.Value >= ExperienceToNextLevel)
        {
          experience = 0;
          continue;
        }

        break;
      }
    }
  }
}
