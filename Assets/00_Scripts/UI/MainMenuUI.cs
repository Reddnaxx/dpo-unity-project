using _00_Scripts.Constants;
using _00_Scripts.Helpers;
using UnityEngine;

namespace _00_Scripts.UI
{
  public class MainMenuUI : MonoBehaviour
  {
    public void OnPlayClick()
    {
      Events.Publish("loadScene", SceneNames.Sandbox);
    }
  }
}