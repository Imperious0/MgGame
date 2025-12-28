using Game.Runtime.Scripts.Items;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime.InGame.Models
{
    [CreateAssetMenu(fileName = "PrefabDatabase", menuName = "LevelEditor/PrefabDatabase")]
    public class PrefabDatabase : ScriptableObject
    {
        public List<EnvironmentEntry> environmentPrefabs = new List<EnvironmentEntry>();
        public List<CollectableEntry> collectablePrefabs = new List<CollectableEntry>();

        [Serializable]
        public struct EnvironmentEntry
        {
            public EnvironmentId id;
            public EnvironmentItem prefab;
        }

        [Serializable]
        public struct CollectableEntry
        {
            public CollectableId id;
            public CollectableItem prefab;
        }

        public EnvironmentItem GetEnvironmentPrefab(EnvironmentId id) => environmentPrefabs.Find(x => x.id == id).prefab;
        public CollectableItem GetCollectablePrefab(CollectableId id) => collectablePrefabs.Find(x => x.id == id).prefab;

        [Serializable]
        public struct CollectableSprite
        {
            public CollectableId id;
            public Sprite sprite;
        }

        public List<CollectableSprite> collectableSprites = new List<CollectableSprite>();

        public Sprite GetCollectableSprite(CollectableId collectableId) => collectableSprites.Find(x => x.id == collectableId).sprite;
    }
}
