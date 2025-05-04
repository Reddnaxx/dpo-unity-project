using _00_Scripts.Constants;
using _00_Scripts.Events;
using _00_Scripts.Helpers;
using UnityEngine;

namespace _00_Scripts.UI
{
  public class MainMenuUI : MonoBehaviour
  {
    public void OnPlayClick()
    {
      EventBus.Publish(new LoadSceneEvent(SceneNames.Sandbox));
    }
  }
}
