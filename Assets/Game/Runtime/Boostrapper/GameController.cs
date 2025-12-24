using Game.SingletonHelper;
using UnityEngine;

namespace Game.Bootstrapper
{
    public class GameController : SingletonBehaviour<GameController>
    {
        protected override void OnAwake()
        {
            Application.targetFrameRate = 60;
            Screen.orientation = ScreenOrientation.Portrait;
        }


    }
}
