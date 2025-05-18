using DG.Tweening;

using UnityEngine;

namespace _00_Scripts.UI
{
  [RequireComponent(typeof(CanvasGroup))]
  public abstract class UIFadeScreen : MonoBehaviour
  {
    private CanvasGroup _canvasGroup;

    protected virtual void Awake()
    {
      _canvasGroup = GetComponent<CanvasGroup>();

      _canvasGroup.alpha = 0;
      _canvasGroup.interactable = false;
      _canvasGroup.blocksRaycasts = false;

      _canvasGroup
        .DOFade(1f, 0.15f)
        .SetUpdate(true)
        .OnComplete(() =>
        {
          _canvasGroup.interactable = true;
          _canvasGroup.blocksRaycasts = true;
        });
    }

    public virtual void Close()
    {
      _canvasGroup.interactable = false;
      _canvasGroup.blocksRaycasts = false;

      _canvasGroup
        .DOFade(0f, 0.15f)
        .SetUpdate(true)
        .OnComplete(() => Destroy(gameObject));
    }
  }
}
