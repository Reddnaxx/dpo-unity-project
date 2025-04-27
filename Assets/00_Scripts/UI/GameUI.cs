using _00_Scripts.Events;
using _00_Scripts.Helpers;

using TMPro;

using UniRx;

using UnityEngine;
using UnityEngine.UI;

namespace _00_Scripts.UI
{
  public class GameUI : MonoBehaviour
  {
    [SerializeField] private TMP_Text levelText;

    [SerializeField] private TMP_Text experienceText;
    [SerializeField] private Image experienceBar;

    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Image healthBar;

    private void Awake()
    {
      EventBus.On<PlayerExpChangeEvent>()
        .Subscribe(evt => UpdateExperience(evt.CurrentExperience, evt.ExperienceToNextLevel, evt.CurrentLevel));

      EventBus.On<PlayerHPChangeEvent>()
        .Subscribe(evt => UpdateHealth(evt.CurrentHealth, evt.MaxHealth));
    }

    private void UpdateHealth(float health, float maxHealth)
    {
      Debug.Log("Health updated: " + health + " / " + maxHealth);

      healthText.text = $"HP: {health} / {maxHealth}";

      healthBar.fillAmount = health / maxHealth;
    }

    private void UpdateExperience(float experience, float experienceToNextLevel, int level)
    {
      Debug.Log("Experience updated: " + experience + " / " +
                experienceToNextLevel + " (Level: " + level + ")");

      levelText.text = $"Уровень: {level}";

      experienceText.text = $"Опыт: {experience} / {experienceToNextLevel}";

      experienceBar.fillAmount = experience / experienceToNextLevel;
    }
  }
}
