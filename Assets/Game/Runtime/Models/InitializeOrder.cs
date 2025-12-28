using Game.Runtime.InGame.Scripts.Controller;
using Game.Runtime.InGame.Scripts.World;
using Game.Runtime.Models;
using Game.Runtime.PanelHandler;
using Game.Runtime.PlayerData;
using System;
using System.Collections.Generic;

namespace Game.Runtime.Assets.Runtime.Models
{
    public static class InitializeOrder
    {
        private static Dictionary<string, IReadOnlyDictionary<Type, int>> _initializeOrderByScenes = new Dictionary<string, IReadOnlyDictionary<Type, int>>()
        {
            {
                SceneNames.InitializeScene,
                new Dictionary<Type, int>()
                {
                    { typeof(PlayerDataController), 0 },
                }
            },
            {
                SceneNames.MainMenuScene,
                new Dictionary<Type, int>()
                {
                    { typeof(PanelController), 0 },
                }
            },
            {
                SceneNames.GamePlayScene,
                new Dictionary<Type, int>()
                {
                    { typeof(InGameController), 0 },
                    { typeof(PanelController), 1 },
                    { typeof(InGameHandler), 2 },
                }
            },
        };

        public static bool TryGetInitializeOrdersByChapter(string sceneName, out IReadOnlyDictionary<Type, int> initializeOrder)
        {
            return _initializeOrderByScenes.TryGetValue(sceneName, out initializeOrder);
        }
    }
}
