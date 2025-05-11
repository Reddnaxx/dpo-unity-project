using _00_Scripts.Game.Rewards;

using UnityEngine;
using UnityEngine.UI;

namespace _00_Scripts.Game.Items
{
  public class ItemView : MonoBehaviour
  {
    [SerializeField] private Image iconImage;
    [SerializeField] private Image bgImage;
    [SerializeField] private float tweenDuration;

    [SerializeField] private LevelUpRewardItem hoverPreview;

    private GameObject _previewGameObject;
    private Item _item;

    public void Init(Item item)
    {
      _item = item;
      iconImage.sprite = item.icon;
    }

    public void OnHoverStart()
    {
      bgImage.color = new Color(bgImage.color.r, bgImage.color.g, bgImage.color.b, 1f);
    }

    public void OnHoverEnd()
    {
      Destroy(_previewGameObject);
      bgImage.color = new Color(bgImage.color.r, bgImage.color.g, bgImage.color.b, 0);
    }

    public void OnClick()
    {
      var preview = Instantiate(hoverPreview, transform);
      preview.Init(_item);

      _previewGameObject = preview.gameObject;
    }
  }
}
