using System;
using _00_Scripts.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _00_Scripts.Scenes
{
    public abstract class SceneEntryPoint : MonoBehaviour
    {
        [SerializeField] private GameObject uiScreen;

        protected UIRoot UIRoot;

        protected virtual void Awake()
        {
            UIRoot = FindFirstObjectByType<UIRoot>();
        }

        public virtual void Init()
        {
            SetScreen();

            if (Debug.isDebugBuild)
            {
                var sceneName = SceneManager.GetActiveScene().name;
                Debug.Log($"[Entry Point] {sceneName} Initialized");
            }
        }

        public virtual void Dispose()
        {
            if (Debug.isDebugBuild)
            {
                var sceneName = SceneManager.GetActiveScene().name;
                Debug.Log($"[Entry Point] {sceneName} Disposed");
            }
        }

        private void SetScreen()
        {
            if (!uiScreen)
            {
                throw new NullReferenceException("UIScreen is not set");
            }

            UIRoot.ClearScreens();
            UIRoot.AddScreen(uiScreen);
        }
    }
}