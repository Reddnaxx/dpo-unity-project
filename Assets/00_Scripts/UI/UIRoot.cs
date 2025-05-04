using DG.Tweening;
using UnityEngine;

namespace _00_Scripts.UI
{
  public class UIRoot : MonoBehaviour
  {
    public const float FadeDuration = 0.5f;
    public CanvasGroup loadingScreenCanvasGroup;

    [SerializeField] private Transform screens;

    private void Start()
    {
      loadingScreenCanvasGroup.alpha = 0;
      loadingScreenCanvasGroup.interactable = false;
      loadingScreenCanvasGroup.blocksRaycasts = false;
    }

    public void ClearScreens()
    {
      for (var i = 0; i < screens.childCount; i++)
        Destroy(screens.GetChild(i).gameObject);
    }

    public void AddScreen(GameObject newScreen)
    {
      Instantiate(newScreen, screens, false);
    }

    public void ShowLoaderSmooth(float time = FadeDuration)
    {
      loadingScreenCanvasGroup.DOFade(1, time).OnStart(() =>
      {
        loadingScreenCanvasGroup.blocksRaycasts = true;
        loadingScreenCanvasGroup.interactable = true;
      });
    }

    public void HideLoaderSmooth(float time = FadeDuration)
    {
      loadingScreenCanvasGroup.DOFade(0, time).OnComplete(() =>
      {
        loadingScreenCanvasGroup.blocksRaycasts = false;
        loadingScreenCanvasGroup.interactable = false;
      });
    }
  }
}