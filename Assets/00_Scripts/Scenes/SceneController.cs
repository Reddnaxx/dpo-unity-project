using System;
using System.Collections;
using _00_Scripts.Constants;
using _00_Scripts.UI;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace _00_Scripts.Scenes
{
  public class SceneController
  {
    private const float SecondsToWait = .5f;

    private readonly UIRoot _uiRoot;
    private SceneEntryPoint _currentEntryPoint;

    public SceneController(UIRoot uiRoot)
    {
      _uiRoot = uiRoot;
    }

    public IObservable<Unit> LoadScene(string sceneName)
    {
      return Observable.FromCoroutine(() => LoadSceneCoroutine(sceneName))
        .First();
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
      _currentEntryPoint?.Dispose();

      _uiRoot.ShowLoaderSmooth();
      yield return new WaitForSeconds(UIRoot.FadeDuration);

      yield return SceneManager.LoadSceneAsync(SceneNames.Boot);
      yield return new WaitForSeconds(SecondsToWait);

      yield return SceneManager.LoadSceneAsync(sceneName);

      _currentEntryPoint = Object.FindFirstObjectByType<SceneEntryPoint>();
      _currentEntryPoint?.Init();

      _uiRoot.HideLoaderSmooth();
    }
  }
}