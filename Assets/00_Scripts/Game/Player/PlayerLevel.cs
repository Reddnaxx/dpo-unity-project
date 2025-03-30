using System;
using UniRx;
using UnityEngine;

namespace _00_Scripts.Game.Player
{
  [Serializable]
  public class PlayerLevel
  {
    [field: SerializeField]
    public float ExperienceToNextLevelModifier { get; private set; } = 1.1f;

    public ReactiveProperty<int> Level { get; private set; }
    public ReactiveProperty<float> CurrentExperience { get; private set; }
    public float ExperienceToNextLevel { get; private set; } = 100;

    public PlayerLevel(int startLevel = 1)
    {
      Level = new ReactiveProperty<int>(startLevel);
      CurrentExperience = new ReactiveProperty<float>(0);
    }

    public void AddExperience(float experience)
    {
      CurrentExperience.Value += experience;

      if (CurrentExperience.Value < ExperienceToNextLevel) return;

      Level.Value++;
      CurrentExperience.Value -= ExperienceToNextLevel;
      
      ExperienceToNextLevel = Mathf
        .RoundToInt(ExperienceToNextLevel * ExperienceToNextLevelModifier);

      if (CurrentExperience.Value >= ExperienceToNextLevel)
      {
        AddExperience(0);
      }
    }
  }
}