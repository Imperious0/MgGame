using Game.SingletonHelper;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Bootstrapper
{
    public class GameController : SingletonBehaviour<GameController>
    {
        private Coroutine _sceneInitializeCoroutine;
        protected override void OnAwake()
        {
            Application.targetFrameRate = 60;
            Screen.orientation = ScreenOrientation.Portrait;

            DontDestroyOnLoad(this.gameObject);

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
            var asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            
            _sceneInitializeCoroutine = null;
        }
    }
}
