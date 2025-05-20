using System;

using _00_Scripts.Events;
using _00_Scripts.Game.Items;
using _00_Scripts.Game.Player;
using _00_Scripts.Helpers;

using TMPro;

using UniRx;

using UnityEngine;
using UnityEngine.UI;

namespace _00_Scripts.UI
{
  public class GameUI : UIFadeScreen
  {
    [SerializeField] private TMP_Text levelText;

    [SerializeField] private TMP_Text experienceText;
    [SerializeField] private Image experienceBar;

    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Image healthBar;

    [SerializeField] private TMP_Text timerText;

    protected override void Awake()
    {
      base.Awake();

      EventBus.On<PlayerExpChangeEvent>()
        .Subscribe(evt => UpdateExperience(evt.CurrentExperience, evt.ExperienceToNextLevel, evt.CurrentLevel))
        .AddTo(this);

      EventBus.On<PlayerHpChangeEvent>()
        .Subscribe(evt => UpdateHealth(evt.CurrentHealth, evt.MaxHealth))
        .AddTo(this);

      Observable.Timer(TimeSpan.FromSeconds(1))
        .Repeat()
        .Subscribe(_ => UpdateTimer())
        .AddTo(this);
    }

    private void UpdateTimer()
    {
      var totalSeconds = Mathf.FloorToInt(Time.timeSinceLevelLoad);
      var minutes = totalSeconds / 60;
      var seconds = totalSeconds % 60;
      timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void UpdateHealth(float health, float maxHealth)
    {
      healthText.text = $"HP: {health:F2} / {maxHealth:F2}";

      healthBar.fillAmount = health / maxHealth;
    }

    private void UpdateExperience(float experience, float experienceToNextLevel, int level)
    {
      levelText.text = $"Уровень: {level}";

      experienceText.text = $"Опыт: {experience} / {experienceToNextLevel}";

      experienceBar.fillAmount = experience / experienceToNextLevel;
    }
  }
}
