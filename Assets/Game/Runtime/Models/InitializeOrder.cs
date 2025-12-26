using Game.Runtime.Models;
using Game.Runtime.PanelHandler;
using Game.Runtime.UI.MainMenu;
using System;
using System.Collections.Generic;

namespace Game.Runtime.Assets.Runtime.Models
{
    public static class InitializeOrder
    {
        private static Dictionary<string, IReadOnlyDictionary<Type, int>> _initializeOrderByScenes = new Dictionary<string, IReadOnlyDictionary<Type, int>>()
        {
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
                    { typeof(PanelController), 0 },
                }
            },
        };

        public static bool TryGetInitializeOrdersByChapter(string sceneName, out IReadOnlyDictionary<Type, int> initializeOrder)
        {
            return _initializeOrderByScenes.TryGetValue(sceneName, out initializeOrder);
        }
    }
}
