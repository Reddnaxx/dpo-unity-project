using System;

using _00_Scripts.Game.Items;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace _00_Scripts.Game.Rewards
{
  public class LevelUpRewardItem : MonoBehaviour
  {
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;

    public void Init(Item item, Action onClick = null)
    {
      if (!item)
      {
        throw new ArgumentNullException(nameof(item));
      }
      
      if (onClick != null)
      {
        var button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(onClick.Invoke);
      }

      icon.sprite = item.icon;
      icon.preserveAspect = true;
      icon.sprite = item.icon;
      nameText.text = item.itemName;
      descriptionText.text = item.description;
    }
  }
}
