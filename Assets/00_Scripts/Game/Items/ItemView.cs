using _00_Scripts.Game.Rewards;

using DG.Tweening;

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
    private Item _item;

    private GameObject _previewGameObject;

    public void Init(Item item)
    {
      _item = item;
      iconImage.sprite = item.icon;
    }

    public void OnHoverStart() => bgImage.color = new Color(bgImage.color.r, bgImage.color.g, bgImage.color.b, 1f);

    public void OnHoverEnd()
    {
      Destroy(_previewGameObject);
      bgImage.color = new Color(bgImage.color.r, bgImage.color.g, bgImage.color.b, 0);
    }

    public void OnClick()
    {
      var preview = Instantiate(hoverPreview, transform);
      preview.Init(_item);
      preview.transform.DOMoveZ(1, 0);

      _previewGameObject = preview.gameObject;
    }
  }
}
