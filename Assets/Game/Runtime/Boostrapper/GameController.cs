using Game.Runtime.InitializeHelper;
using Game.Runtime.UI.InputBlocker;
using Game.SingletonHelper;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Runtime.Bootstrapper
{
    public class GameController : SingletonBehaviour<GameController>
    {
        private Coroutine _sceneInitializeCoroutine;
        public string ActiveSceneName { get; private set; }
        protected override void OnAwake()
        {
            Application.targetFrameRate = 60;
            Screen.orientation = ScreenOrientation.Portrait;

            DontDestroyOnLoad(this.gameObject);

            ActiveSceneName = Models.SceneNames.InitializeScene;

            LoadScene(Models.SceneNames.MainMenuScene);
        }

        public void LoadScene(string sceneName)
        {
            if(_sceneInitializeCoroutine != null)
            {
                Debug.LogWarning("A scene is already loading.");
                return;
            } 

            _sceneInitializeCoroutine = StartCoroutine(LoadSceneCoroutine(sceneName));
        }

        private System.Collections.IEnumerator LoadSceneCoroutine(string sceneName)
        {
            InputBlocker.Instance?.BlockInteractions();

            var asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            ActiveSceneName = sceneName;

            yield return new WaitUntil(() => InitializeController.Instance && InitializeController.Instance.InitializeCompleted);

            _sceneInitializeCoroutine = null;
            InputBlocker.Instance?.Release();
        }
    }
}
