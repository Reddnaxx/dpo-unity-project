using System;
using System.Collections.Generic;

namespace _00_Scripts.Game.Items
{
  public static class UpgradeTitleMap
  {
    private static readonly Dictionary<UpgradeType, string> UpgradeTitles = new()
    {
      { UpgradeType.Health, "Здоровье" },
      { UpgradeType.HealthMultiplier, "Здоровье" },
      { UpgradeType.Attack, "Урон" },
      { UpgradeType.AttackMultiplier, "Урон" },
      { UpgradeType.Speed, "Скорость" },
      { UpgradeType.SpeedMultiplier, "Скорость" },
      { UpgradeType.PhysicalResistance, "Сопротивление физическому урону" },
      { UpgradeType.FireResistance, "Сопротивление огненному урону" },
      { UpgradeType.IceResistance, "Сопротивление ледяному урону" },
      { UpgradeType.PoisonResistance, "Сопротивление ядовитому урону" },
    };

    public static string GetTitle(UpgradeType upgrade)
      => UpgradeTitles.GetValueOrDefault(upgrade, "Unknown Upgrade");

    public static string GetDescription(UpgradeType upgrade, float value)
    {
      var isMultiplier = IsMultiplier(upgrade);
      var isNegative = (isMultiplier && value >= 1) || (!isMultiplier && value > 0);

      var verb = isNegative
        ? "Увеличивает"
        : "Уменьшает";

      var absValue = Math.Abs(value);
      var title = GetTitle(upgrade).ToLowerInvariant();
      var rawFormatted = FormatValue(upgrade, absValue);
      var color = isNegative ? "green" : "red";
      var coloredValue = $"<color={color}>{rawFormatted}</color>";

      return $"{verb} {title} на {coloredValue}";
    }

    private static string FormatValue(UpgradeType upgrade, float value)
    {
      if (IsResistance(upgrade))
      {
        return $"{Math.Abs(value * 100f):0.#}%";
      }

      return IsMultiplier(upgrade)
        ? $"{Math.Abs((value - 1f) * 100f):0.#}%"
        : $"{Math.Abs(value):0.#}";
    }

    private static bool IsMultiplier(UpgradeType upgrade) =>
      upgrade.ToString().Contains("multiplier", StringComparison.OrdinalIgnoreCase);

    private static bool IsResistance(UpgradeType upgrade) =>
      upgrade.ToString().Contains("resistance", StringComparison.OrdinalIgnoreCase);
  }
}
