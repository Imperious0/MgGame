using Game.Runtime.InGame.Models;
using Game.Runtime.InGame.Models.Level;
using Game.Runtime.InGame.Scripts.Interfaces;
using Game.Runtime.InitializeHelper;
using Game.Runtime.Scripts;
using Game.Runtime.Scripts.Items;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Runtime.InGame.Scripts
{
    internal class InGameHandler : MonoBehaviour, IInitializable
    {
        [SerializeField] private PrefabDatabase _prefabDatabase;

        [SerializeField] private Transform _mapRoot;
        [SerializeField] private Transform _environmentRoot;
        [SerializeField] private Transform _collectableRoot;


        private void Awake()
        {
            InitializeController.Instance.RegisterInitialize(this);
        }

        public void Initialize()
        {
            InGameController.Instance.LevelData
        }

        void SyncSceneWithData(LevelData data)
        {
            if (_mapRoot != null) ApplyDataToTransform(_mapRoot.transform, data.MapData);
            if (Camera.main != null)
            {
                ApplyDataToTransform(Camera.main.transform, data.CameraData.CameraTRS);
                Camera.main.orthographicSize = data.CameraData.CameraSize;
            }

            if (_environmentRoot != null)
            {
                ApplyDataToTransform(_environmentRoot.transform, data.EnvironmentRootData);
                if (data.EnvironmentData != null)
                {
                    foreach(var kvp in data.EnvironmentData)
                    {
                        foreach(var kvp2 in kvp.Value)
                        {
                            EnvironmentItem envItem = CreateItem<EnvironmentItem>(kvp.Key, _environmentRoot.transform);
                            ApplyDataToTransform(envItem.EnvironmentTransform, kvp2);
                        }
                    }
                }

            }

            if (_collectableRoot != null)
            {
                ApplyDataToTransform(_collectableRoot.transform, data.CollectablesRootData);
                if (data.CollectableData != null)
                {
                    foreach (var kvp in data.CollectableData)
                    {
                        foreach (var kvp2 in kvp.Value)
                        {
                            CollectableItem envItem = CreateItem<CollectableItem>(kvp.Key, _collectableRoot.transform);
                            ApplyDataToTransform(envItem.CollectableTransform, kvp2);
                        }
                    }
                }
            }


        }
        TItem CreateItem<TItem> (EnvironmentId environmentId, Transform parent) where TItem : Object
        {
            return Object.Instantiate<TItem>(_prefabDatabase.GetEnvironmentPrefab(environmentId) as TItem, parent: parent);
        }
        TItem CreateItem<TItem>(CollectableId collectableId, Transform parent) where TItem : Object
        {
            return Object.Instantiate<TItem>(_prefabDatabase.GetCollectablePrefab(collectableId) as TItem, parent: parent);
        }
       
        void ApplyDataToTransform(Transform target, GameItemData data)
        {
            target.position = data.Position;
            target.eulerAngles = data.Rotation;
            target.localScale = data.Scale;
        }
    }
}
