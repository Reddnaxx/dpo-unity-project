using System;

using _00_Scripts.Constants;
using _00_Scripts.Events;
using _00_Scripts.Game.Weapon.Core;
using _00_Scripts.Helpers;
using _00_Scripts.Scenes;
using _00_Scripts.UI;

using UniRx;

using UnityEngine;
using UnityEngine.SceneManagement;

using Object = UnityEngine.Object;

namespace _00_Scripts.Core
{
  public class Bootstrap
  {
    private static Bootstrap _instance;

    private SceneController _sceneController;

    private WeaponType _selectedWeaponType;

    public UIRoot UIRoot
    {
      get;
      private set;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void OnBeforeSceneLoad()
    {
      _instance = new Bootstrap();

      _instance.InitGame();
    }

    private void InitGame()
    {
      InitBaseEvents();
      InstantiateBaseObjects();

      _sceneController = new SceneController(UIRoot);

      StartGame().Subscribe();
    }

    private IObservable<Unit> StartGame()
    {
#if UNITY_EDITOR
      var currentScene = SceneManager.GetActiveScene().name;

      if (currentScene != SceneNames.Boot &&
          currentScene != SceneNames.MainMenu)
      {
        SceneManager.LoadScene(SceneNames.Boot);
        return _sceneController.LoadScene(currentScene);
      }
#endif

      return _sceneController.LoadScene(SceneNames.MainMenu);
    }

    private void InitBaseEvents()
    {
      EventBus
        .On<LoadSceneEvent>()
        .Subscribe(evt => LoadScene(evt.SceneName));

      EventBus.On<PlayerSelectWeaponEvent>().Subscribe(evt => _selectedWeaponType = evt.SelectedWeapon);
    }

    private void InstantiateBaseObjects()
    {
      UIRoot = Object.Instantiate(Resources.Load<UIRoot>("UI/UIRoot"));
      Object.DontDestroyOnLoad(UIRoot.gameObject);

      var eventSystem = Object.Instantiate(Resources.Load("UI/EventSystem"));
      Object.DontDestroyOnLoad(eventSystem);
    }

    private void LoadScene(string sceneName)
    {
      _sceneController.SetPlayerWeapon(_selectedWeaponType);

      _sceneController.LoadScene(sceneName)
        .Subscribe();
    }
  }
}
