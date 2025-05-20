using _00_Scripts.Constants;
using _00_Scripts.Events;
using _00_Scripts.Helpers;

using TMPro;

using UnityEngine;

namespace _00_Scripts.UI
{
  public class GameOverUI : UIFadeScreen
  {
    [SerializeField] private TMP_Text resultsText;

    public void OnRestartButtonClick() => EventBus.Publish(new LoadSceneEvent(SceneNames.Map));

    public void OnMainMenuButtonClick() => EventBus.Publish(new LoadSceneEvent(SceneNames.MainMenu));
  }
}
