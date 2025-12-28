using Game.Runtime.InGame.Models;
using Game.Runtime.InGame.Scripts.Controller;
using Game.Runtime.InGame.Scripts.Interfaces;
using Game.Runtime.InGame.Scripts.Utility;
using Game.Runtime.Scripts.Items;
using UnityEngine;

namespace Game.Runtime.InGame.Scripts.World
{
    internal class InGameCollectableHandler : MonoBehaviour, IUpdatable
    {
        [SerializeField] private Transform _collectableRoot;

        private CollectableHandler _collectableHandler;
        private PrefabDatabase _prefabDatabase;
        public void Initialize(CollectableHandler collectableHandler, PrefabDatabase prefabDatabase)
        {
            _collectableHandler = collectableHandler;
            _prefabDatabase = prefabDatabase;

            CreateCollectables();

            _collectableHandler.OnItemCollectedEvent += OnItemCollected;
        }

        public void Dispose()
        {
            _collectableHandler.OnItemCollectedEvent -= OnItemCollected;
        }

        public void Tick(float deltaTime, float unscaledDeltaTime)
        {

        }

        private void CreateCollectables()
        {
            if (_collectableRoot != null)
            {
                GameDataUtils.ApplyDataToTransform(_collectableRoot.transform, _collectableHandler.CollectableHolderData);
                if (_collectableHandler.ActiveCollectableDatas != null)
                {
                    foreach (var kvp in _collectableHandler.ActiveCollectableDatas)
                    {
                        foreach (var kvp2 in kvp.Value)
                        {
                            CollectableItem collectableItem = CreateItem<CollectableItem>(kvp.Key, _collectableRoot.transform);
                            collectableItem.Initialize(kvp2.ItemId);
                            GameDataUtils.ApplyDataToTransform(collectableItem.CollectableTransform, kvp2.GameItemData);
                        }
                    }
                }
            }
        }

        TItem CreateItem<TItem>(CollectableId collectableId, Transform parent) where TItem : Object
        {
            return Object.Instantiate<TItem>(_prefabDatabase.GetCollectablePrefab(collectableId) as TItem, parent: parent);
        }

        private void OnItemCollected(int itemIndex) 
        {

        }
    }
}
