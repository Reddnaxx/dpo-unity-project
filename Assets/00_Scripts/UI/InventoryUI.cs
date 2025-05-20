using System.Text;

using _00_Scripts.Game.Entity;
using _00_Scripts.Game.Items;
using _00_Scripts.Game.Player;

using TMPro;

using UnityEngine;
using UnityEngine.InputSystem;

namespace _00_Scripts.UI
{
  public class InventoryUI : UIFadeScreen
  {
    [SerializeField] private ItemView itemPrefab;
    [SerializeField] private RectTransform itemsListPanel;
    [SerializeField] private TMP_Text statsText;

    private static Inventory PlayerInventory => PlayerCharacter.Inventory;
    private static float PlayerHealth => PlayerCharacter.HealthProperty.Value;
    private static IStats PlayerStats => PlayerCharacter.Stats;

    private UIRoot _uiRoot;
    private PlayerInput _playerInput;

    protected override void Awake()
    {
      base.Awake();

      _uiRoot = FindFirstObjectByType<UIRoot>();
      _playerInput = FindFirstObjectByType<PlayerInput>();

      UpdateItems();
      UpdateStats();
    }

    public void CloseMenu()
    {
      _uiRoot.RemoveScreen(gameObject);
      _playerInput.ActivateInput();
      Time.timeScale = 1f;
    }

    private void UpdateStats()
    {
      var result = new StringBuilder();

      result.AppendLine($"HP: {PlayerHealth:F2} / {PlayerStats.MaxHealth:F2}");
      result.AppendLine($"Урон: {PlayerStats.Attack}");
      result.AppendLine($"Скорострельнсоть: {PlayerStats.AttackSpeed}");
      result.AppendLine($"Скорость: {PlayerStats.Speed}");
      result.AppendLine($"Сопротивление физическому урону: {Mathf.Abs(PlayerStats.PhysicalResistance * 100)}%");
      result.AppendLine($"Сопротивление урону от огня: {Mathf.Abs(PlayerStats.FireResistance * 100)}%");
      result.AppendLine($"Сопротивление урону от льда: {Mathf.Abs(PlayerStats.IceResistance * 100)}%");
      result.AppendLine($"Сопротивление урону от яда: {Mathf.Abs(PlayerStats.PoisonResistance * 100)}%");

      statsText.text = result.ToString();
    }

    private void UpdateItems()
    {
      foreach (Transform child in itemsListPanel)
      {
        Destroy(child.gameObject);
      }

      foreach (var item in PlayerInventory.Items)
      {
        var itemUI = Instantiate(itemPrefab, itemsListPanel);
        itemUI.Init(item);
      }
    }
  }
}
