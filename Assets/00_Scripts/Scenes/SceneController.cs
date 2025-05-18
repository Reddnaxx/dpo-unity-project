using System;
using System.Collections;

using _00_Scripts.Constants;
using _00_Scripts.Game.Weapon.Core;
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

    private WeaponType _selectedWeaponType;

    public SceneController(UIRoot uiRoot)
    {
      _uiRoot = uiRoot;
    }

    public IObservable<Unit> LoadScene(string sceneName) =>
      Observable.FromCoroutine(() => LoadSceneCoroutine(sceneName))
        .First();

    public void SetPlayerWeapon(WeaponType weaponType) => _selectedWeaponType = weaponType;

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
      _currentEntryPoint?.Dispose();

      _uiRoot.ShowLoaderSmooth();
      yield return new WaitForSeconds(UIRoot.FadeDuration);

      yield return SceneManager.LoadSceneAsync(SceneNames.Boot);
      yield return new WaitForSeconds(SecondsToWait);

      yield return SceneManager.LoadSceneAsync(sceneName);

      _currentEntryPoint = Object.FindFirstObjectByType<SceneEntryPoint>();

      if (_currentEntryPoint is GameEntryPoint gameEntryPoint) gameEntryPoint.SetWeapon(_selectedWeaponType);

      _currentEntryPoint?.Init();

      _uiRoot.HideLoaderSmooth();
    }
  }
}
