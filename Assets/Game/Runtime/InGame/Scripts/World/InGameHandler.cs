using Game.Runtime.InGame.Models;
using Game.Runtime.InGame.Models.Level;
using Game.Runtime.InGame.Scripts.Controller;
using Game.Runtime.InGame.Scripts.Utility;
using Game.Runtime.InitializeHelper;
using Game.Runtime.Scripts.Items;
using UnityEngine;

namespace Game.Runtime.InGame.Scripts.World
{
    internal class InGameHandler : MonoBehaviour, IInitializable
    {
        public bool MarkDisposeOnSceneChange => true;

        [SerializeField] private PrefabDatabase _prefabDatabase;

        [SerializeField] private Transform _mapRoot;
        [SerializeField] private Transform _environmentRoot;

        [SerializeField] private InGameCollectableHandler _inGameCollectableHandler;


        private void Awake()
        {
            InitializeController.Instance.RegisterInitialize(this);
        }

        public void Initialize()
        {
            _inGameCollectableHandler.Initialize(InGameController.Instance.CollectableHandler, _prefabDatabase);

            InGameController.Instance.GameUpdateHandler.Register(_inGameCollectableHandler);

            SyncSceneWithData(InGameController.Instance.LevelData);
        }

        public void Dispose()
        {
            _inGameCollectableHandler.Dispose();
        }

        void SyncSceneWithData(LevelData data)
        {
            if (_mapRoot != null) GameDataUtils.ApplyDataToTransform(_mapRoot.transform, data.MapData);
            if (Camera.main != null)
            {
                GameDataUtils.ApplyDataToTransform(Camera.main.transform, data.CameraData.CameraTRS);
                Camera.main.orthographicSize = data.CameraData.CameraSize;
            }

            if (_environmentRoot != null)
            {
                GameDataUtils.ApplyDataToTransform(_environmentRoot.transform, data.EnvironmentRootData);
                if (data.EnvironmentData != null)
                {
                    int envIndex = 0;
                    foreach(var kvp in data.EnvironmentData)
                    {
                        foreach(var kvp2 in kvp.Value)
                        {
                            EnvironmentItem envItem = CreateItem<EnvironmentItem>(kvp.Key, _environmentRoot.transform);
                            envItem.Initialize(envIndex++);
                            GameDataUtils.ApplyDataToTransform(envItem.EnvironmentTransform, kvp2);
                        }
                    }
                }

            }

        }
        TItem CreateItem<TItem> (EnvironmentId environmentId, Transform parent) where TItem : Object
        {
            return Object.Instantiate<TItem>(_prefabDatabase.GetEnvironmentPrefab(environmentId) as TItem, parent: parent);
        }
        
       
        
    }
}
