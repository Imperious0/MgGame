using Game.Runtime.InGame.Models;
using Game.Runtime.InGame.Scripts.Controller;
using Game.Runtime.InGame.Scripts.Interfaces;
using Game.Runtime.InGame.Scripts.Utility;
using Game.Runtime.Scripts.Items;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime.InGame.Scripts.World
{
    internal class InGameCollectableHandler : MonoBehaviour, IUpdatable
    {
        [SerializeField] private Transform _collectableRoot;

        private CollectableHandler _collectableHandler;
        private PrefabDatabase _prefabDatabase;

        private Dictionary<CollectableId, List<CollectableItem>> _collectableItems;
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
                    _collectableItems = new Dictionary<CollectableId, List<CollectableItem>>();
                    foreach (var kvp in _collectableHandler.ActiveCollectableDatas)
                    {
                        List<CollectableItem> collectableItems = new List<CollectableItem>();
                        foreach (var kvp2 in kvp.Value)
                        {
                            CollectableItem collectableItem = CreateItem<CollectableItem>(kvp.Key, _collectableRoot.transform);
                            collectableItem.Initialize(kvp2.ItemId);
                            GameDataUtils.ApplyDataToTransform(collectableItem.CollectableTransform, kvp2.GameItemData);

                            collectableItems.Add(collectableItem);
                        }
                        _collectableItems.Add(kvp.Key, collectableItems);
                    }
                }
            }
        }

        TItem CreateItem<TItem>(CollectableId collectableId, Transform parent) where TItem : Object
        {
            return Object.Instantiate<TItem>(_prefabDatabase.GetCollectablePrefab(collectableId) as TItem, parent: parent);
        }

        private void OnItemCollected(CollectableId collectableId, int itemIndex) 
        {
            if(_collectableItems.TryGetValue(collectableId, out List<CollectableItem> collectableItems))
            {
                int collectableItemIndex = collectableItems.FindIndex(t => t.CollectableId == collectableId && t.ItemId == itemIndex);

                if(collectableItemIndex >= 0)
                {
                    CollectableItem collectableItem = collectableItems[collectableItemIndex];

                    collectableItems.RemoveAt(collectableItemIndex);

                    collectableItem.Dispose();

                    Object.Destroy(collectableItem);

                }

                if (collectableItems.Count <= 0) _collectableItems.Remove(collectableId);
            }
        }
    }
}
