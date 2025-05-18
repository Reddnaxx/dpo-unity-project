using _00_Scripts.Constants;
using _00_Scripts.Events;
using _00_Scripts.Helpers;

using UnityEngine;
using UnityEngine.InputSystem;

namespace _00_Scripts.UI
{
  public class ToMenuApproveUI : UIFadeScreen
  {
    private PlayerInput _playerInput;

    protected override void Awake()
    {
      base.Awake();
      _playerInput = FindFirstObjectByType<PlayerInput>();
    }

    public void Approve()
    {
      Close();
      EventBus.Publish(new LoadSceneEvent(SceneNames.MainMenu));
      Time.timeScale = 1f;
      _playerInput.ActivateInput();
    }

    public void Cancel()
    {
      Close();
      Time.timeScale = 1f;
      _playerInput.ActivateInput();
    }
  }
}
