using Game.Runtime.InGame.Models;
using Game.Runtime.InGame.Models.Level;
using Game.Runtime.InGame.Scripts.Interfaces;
using Game.Runtime.InGame.Scripts.Models;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Runtime.InGame.Scripts.Controller
{
    internal struct CollectableData
    {
        public int ItemId;
        public GameItemData GameItemData;

        public CollectableData(int itemId, GameItemData gameItemData)
        {
            this.ItemId = itemId;
            this.GameItemData = gameItemData;
        }
    }
    internal class CollectableHandler : IUpdatable
    {
        private readonly Camera _cam;
        private readonly Plane _detectionPlane;

        public event Action<int> OnItemCollectedEvent;

        private Action _onAllCollectedEvent;
        private Action _onSlotFullFailEvent;

        public GameItemData CollectableHolderData { get; private set; }
        private IReadOnlyDictionary<CollectableId, List<GameItemData>> _initialCollectableDatas;
        public Dictionary<CollectableId, List<CollectableData>> ActiveCollectableDatas { get; private set; }

        public Dictionary<CollectableId, List<int>> CollectableSlots { get; private set; }

        private bool _doneInteractions;

        public CollectableHandler(IReadOnlyDictionary<CollectableId, List<GameItemData>> collectableData, GameItemData collectableHolderData, Action onAllCollectedEvent = null, Action onSlotFullFailEvent = null)
        {
            _cam = Camera.main;

            _detectionPlane = new Plane(Vector3.back, Vector3.zero);

            _initialCollectableDatas = collectableData;
            CollectableHolderData = collectableHolderData;

            _onAllCollectedEvent = onAllCollectedEvent;
            _onSlotFullFailEvent = onSlotFullFailEvent;

            ActiveCollectableDatas = new Dictionary<CollectableId, List<CollectableData>>();
            CollectableSlots = new Dictionary<CollectableId, List<int>>();

            _doneInteractions = false;
        }

        public void Initialize()
        {
            int collectableIndex = 0;
            foreach (var kvp in _initialCollectableDatas)
            {
                List<CollectableData> collectableDatas = new List<CollectableData>(kvp.Value.Count);

                foreach (var kvp2 in kvp.Value)
                {
                    collectableDatas.Add(new CollectableData(collectableIndex++, kvp2));
                }

                ActiveCollectableDatas.Add(kvp.Key, collectableDatas);
            }
        }

        public void Dispose()
        {
        }

        public void Tick(float deltaTime, float unscaledDeltaTime)
        {
            if (_doneInteractions) return;

            var pointer = Pointer.current;

            if (pointer == null) return;

            if (pointer.press.wasPressedThisFrame)
            {
                Vector2 screenPos = pointer.position.ReadValue();
                ProcessInput(screenPos);
            }
        }

        private void ProcessInput(Vector2 screenPosition)
        {
            Ray ray = _cam.ScreenPointToRay(screenPosition);

            if (_detectionPlane.Raycast(ray, out float enter))
            {
                Vector3 worldPoint = ray.GetPoint(enter);

                Collider2D hit = Physics2D.OverlapPoint(worldPoint);

                if (hit != null)
                {
                    OnObjectInteract(hit.gameObject);
                }
            }
        }

        private void OnObjectInteract(GameObject interactedObject)
        {
            ICollectable collectable = interactedObject.GetComponent<ICollectable>();
            if (collectable == null) return;

            if (!ActiveCollectableDatas.TryGetValue(collectable.CollectableId, out List<CollectableData> collectableData)) return;
            int collectableItemIndex = collectableData.FindIndex(t => t.ItemId == collectable.ItemId);
            if (collectableItemIndex == -1) return;

            if (!CollectableSlots.TryAdd(collectable.CollectableId, new List<int>() { collectable.ItemId }))
            {
                CollectableSlots[collectable.CollectableId].Add(collectable.ItemId);
            }

            OnItemCollectedEvent?.Invoke(collectable.ItemId);

            if (CollectableSlots[collectable.CollectableId].Count >= GameConstants.GameSettings.MatchItemCount)
            {
                CollectableSlots[collectable.CollectableId].Clear();
            }
            else
            {
                int emptySlotCount = GameConstants.GameSettings.EmptySlotCount;

                foreach (var kvp in CollectableSlots)
                {
                    emptySlotCount -= kvp.Value.Count;
                    if (emptySlotCount <= 0)
                    {
                        _doneInteractions = true;
                        _onSlotFullFailEvent?.Invoke();
                    }
                }

                collectableData.RemoveAt(collectableItemIndex);
                if(collectableData.Count <= 0) ActiveCollectableDatas.Remove(collectable.CollectableId);

                if(ActiveCollectableDatas.Count <= 0)
                {
                    _doneInteractions = true;
                    _onAllCollectedEvent?.Invoke();
                }
            }
        }
    }
}
