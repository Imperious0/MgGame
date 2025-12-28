using Game.Runtime.InGame.Models;
using Game.Runtime.InitializeHelper;
using Game.Runtime.JsonUtils.JsonConverters;
using Game.Runtime.UI.InputBlocker;
using Game.SingletonHelper;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;

namespace Game.Runtime.Bootstrapper
{
    public class GameController : SingletonBehaviour<GameController>
    {
        private Coroutine _sceneLoadCoroutine;
        public string ActiveSceneName { get; private set; }

        public PrefabDatabase PrefabDatabase { get; private set; }

        protected override void OnAwake()
        {
            Application.targetFrameRate = 60;
            Screen.orientation = ScreenOrientation.Portrait;
            EnhancedTouchSupport.Enable();

            DontDestroyOnLoad(this.gameObject);

            PrefabDatabase = GetPrefabDatabase();
            if (PrefabDatabase == null) throw new System.Exception($"Cant find PrefabDatabase!");

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                Converters = new JsonConverter[]
                {
                    new Vector3Converter(),
                }
            };

            ActiveSceneName = Models.SceneNames.InitializeScene;

            StartCoroutine(FinalizeInitializeScene());
        }

        private PrefabDatabase GetPrefabDatabase()
        {
            return Resources.Load<PrefabDatabase>("PrefabDatabase");
        }

        private System.Collections.IEnumerator FinalizeInitializeScene()
        {
            yield return new WaitUntil(() => InitializeController.Instance && InitializeController.Instance.InitializeCompleted);
            LoadScene(Models.SceneNames.MainMenuScene);
        }

        public void LoadScene(string sceneName)
        {
            if(_sceneLoadCoroutine != null)
            {
                Debug.LogWarning("A scene is already loading.");
                return;
            } 

            _sceneLoadCoroutine = StartCoroutine(LoadSceneCoroutine(sceneName));
        }

        private System.Collections.IEnumerator LoadSceneCoroutine(string sceneName)
        {
            InputBlocker.Instance?.BlockInteractions();

            InitializeController.Instance.Dispose();
            
            ActiveSceneName = sceneName;

            var asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            yield return new WaitUntil(() => InitializeController.Instance && InitializeController.Instance.InitializeCompleted);

            _sceneLoadCoroutine = null;
            InputBlocker.Instance?.Release();
        }
    }
}
