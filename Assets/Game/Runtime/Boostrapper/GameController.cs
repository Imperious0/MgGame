using Game.SingletonHelper;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Bootstrapper
{
    public class GameController : SingletonBehaviour<GameController>
    {
        protected override void OnAwake()
        {
            Application.targetFrameRate = 60;
            Screen.orientation = ScreenOrientation.Portrait;

            DontDestroyOnLoad(this.gameObject);

            SceneManager.LoadScene(Models.SceneNames.MainMenuScene);
        }

    }
}
