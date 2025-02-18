using _00_Scripts.Helpers;
using _00_Scripts.Scenes;
using _00_Scripts.UI;
using UniRx;
using UnityEngine;

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
            InitEvents();
            InstantiateRootObjects();

            _sceneController = new SceneController(UIRoot);

            Object.DontDestroyOnLoad(UIRoot.gameObject);
        }

        private void InitEvents()
        {
            Events.On<string>("loadScene", nameof(LoadScene)).Subscribe(LoadScene);
        }

        private void InstantiateRootObjects()
        {
            UIRoot = Object.Instantiate(Resources.Load<UIRoot>("UI/UIRoot"));
        }

        private void LoadScene(string sceneName)
        {
            _sceneController.LoadScene(sceneName).Subscribe(_ => { UIRoot.ClearScreens(); });
        }
    }
}