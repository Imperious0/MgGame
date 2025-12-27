using Game.AotHelper;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Game.Runtime.InGame.Models.Level
{
    public readonly struct LevelData
    {
        public readonly GameItemData MapData;
        public readonly CameraData CameraData;

        public readonly GameItemData EnvironmentRootData;
        public readonly GameItemData CollectablesRootData;

        [Preserve(typeof(AotEnsureDictionary<EnvironmentId, List<GameItemData>>))]
        public readonly IReadOnlyDictionary<EnvironmentId, List<GameItemData>> EnvironmentData;
        [Preserve(typeof(AotEnsureDictionary<CollectableId, List<GameItemData>>))]
        public readonly IReadOnlyDictionary<CollectableId, List<GameItemData>> CollectableData;

        [JsonConstructor]
        public LevelData(GameItemData mapData, CameraData cameraData, 
            GameItemData environmentRootData, GameItemData collectablesRootData,
            IReadOnlyDictionary<EnvironmentId, List<GameItemData>> environmentData, IReadOnlyDictionary<CollectableId, List<GameItemData>> collectableData)
        {
            MapData = mapData;
            CameraData = cameraData;
            EnvironmentRootData = environmentRootData;
            CollectablesRootData = collectablesRootData;
            EnvironmentData = environmentData;
            CollectableData = collectableData;
        }
    }
}
