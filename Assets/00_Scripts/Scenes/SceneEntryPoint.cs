using System;
using _00_Scripts.UI;
using UnityEngine;

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

        protected virtual void Start()
        {
            SetScreen();
        }

        private void SetScreen()
        {
            if (uiScreen == null)
            {
                throw new NullReferenceException("UIScreen is not set");
            }

            UIRoot.ClearScreens();
            UIRoot.AddScreen(uiScreen);
        }
    }
}