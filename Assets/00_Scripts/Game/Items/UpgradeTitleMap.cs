using System;
using System.Collections.Generic;

namespace _00_Scripts.Game.Items
{
  public static class UpgradeTitleMap
  {
    private static readonly Dictionary<StatUpgradeType, string> UpgradeTitles = new()
    {
      { StatUpgradeType.Health, "Здоровье" },
      { StatUpgradeType.HealthMultiplier, "Здоровье" },
      { StatUpgradeType.Attack, "Урон" },
      { StatUpgradeType.AttackMultiplier, "Урон" },
      { StatUpgradeType.Speed, "Скорость" },
      { StatUpgradeType.SpeedMultiplier, "Скорость" },
      { StatUpgradeType.AttackSpeed , "Скорострельность"},
      { StatUpgradeType.AttackSpeedMultiplier , "Скорострельность"},
      { StatUpgradeType.PhysicalResistance, "Сопротивление физическому урону" },
      { StatUpgradeType.FireResistance, "Сопротивление огненному урону" },
      { StatUpgradeType.IceResistance, "Сопротивление ледяному урону" },
      { StatUpgradeType.PoisonResistance, "Сопротивление ядовитому урону" },
    };

    private static readonly Dictionary<WeaponUpgradeType, string> WeaponUpgradeTitles = new()
    {
      { WeaponUpgradeType.Homing, "Самонаводящиеся снаряды" },
      { WeaponUpgradeType.Bounce, "Отскакивающие снаряды" },
    };

    public static string GetTitle(Upgrade upgrade)
    {
      return upgrade.category == UpgradeCategory.Weapon
        ? WeaponUpgradeTitles.GetValueOrDefault(upgrade.weaponType, "Unknown Weapon Upgrade")
        : UpgradeTitles.GetValueOrDefault(upgrade.statType, "Unknown Upgrade");
    }

    private static string GetStatDescription(Upgrade upgrade)
    {
      var isMultiplier = IsMultiplier(upgrade);
      var isNegative = (isMultiplier && upgrade.value >= 1) || (!isMultiplier && upgrade.value > 0);

      var verb = isNegative
        ? "Увеличивает"
        : "Уменьшает";

      var title = GetTitle(upgrade).ToLowerInvariant();
      var rawFormatted = FormatValue(upgrade);
      var color = isNegative ? "green" : "red";
      var coloredValue = $"<color={color}>{rawFormatted}</color>";

      return $"{verb} {title} на {coloredValue}";
    }

    private static string GetWeaponUpgradeDescription(Upgrade upgrade)
    {
      var title = GetTitle(upgrade).ToLowerInvariant();

      return $"Добавляет <color=green>{title}</color>";
    }

    public static string GetDescription(Upgrade upgrade)
    {
      return upgrade.category == UpgradeCategory.Weapon
        ? GetWeaponUpgradeDescription(upgrade)
        : GetStatDescription(upgrade);
    }

    private static string FormatValue(Upgrade upgrade)
    {
      if (IsResistance(upgrade))
      {
        return $"{Math.Abs(upgrade.value * 100f):0.#}%";
      }

      return IsMultiplier(upgrade)
        ? $"{Math.Abs((upgrade.value - 1f) * 100f):0.#}%"
        : $"{Math.Abs(upgrade.value):0.#}";
    }

    private static bool IsMultiplier(Upgrade upgrade) =>
      upgrade.statType.ToString().Contains("multiplier", StringComparison.OrdinalIgnoreCase);

    private static bool IsResistance(Upgrade upgrade) =>
      upgrade.statType.ToString().Contains("resistance", StringComparison.OrdinalIgnoreCase);
  }
}
