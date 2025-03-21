using System;
using _00_Scripts.Constants;
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

    public UIRoot UIRoot { get; private set; }

    private SceneController _sceneController;

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
        return Observable.ReturnUnit();
      }
#endif

      return _sceneController.LoadScene(SceneNames.MainMenu);
    }

    private void InitBaseEvents()
    {
      Events.On<string>("loadScene", nameof(LoadScene)).Subscribe(LoadScene);
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
      _sceneController.LoadScene(sceneName)
        .Subscribe(_ => { UIRoot.ClearScreens(); });
    }
  }
}