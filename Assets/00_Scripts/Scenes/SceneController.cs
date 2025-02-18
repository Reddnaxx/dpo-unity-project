using System;
using System.Collections;
using _00_Scripts.Constants;
using _00_Scripts.UI;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _00_Scripts.Scenes
{
    public class SceneController
    {
        private readonly CanvasGroup _loadingScreenCanvasGroup;
        private const int SecondsToWait = 1;

        private readonly UIRoot _uiRoot;

        public SceneController(UIRoot uiRoot)
        {
            _uiRoot = uiRoot;
        }

        public IObservable<Unit> LoadScene(string sceneName)
        {
            return Observable.FromCoroutine(() => LoadSceneCoroutine(sceneName)).First();
        }

        private IEnumerator LoadSceneCoroutine(string sceneName)
        {
            _uiRoot.ShowLoaderSmooth();
            yield return new WaitForSeconds(UIRoot.FadeDuration);

            yield return SceneManager.LoadSceneAsync(SceneNames.Boot);
            yield return new WaitForSeconds(SecondsToWait);

            yield return SceneManager.LoadSceneAsync(sceneName);
            _uiRoot.HideLoaderSmooth();
        }
    }
}