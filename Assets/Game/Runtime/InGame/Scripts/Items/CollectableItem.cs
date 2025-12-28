using Game.Runtime.InGame.Models;
using Game.Runtime.InGame.Scripts.Interfaces;
using UnityEngine;

namespace Game.Runtime.Scripts.Items
{
    public class CollectableItem : MonoBehaviour, ICollectable
    {
        [SerializeField] private CollectableId _collectableId;
        public int ItemId { get; private set; }
        public CollectableId CollectableId => _collectableId;
        public Transform CollectableTransform => transform;

        public void Initialize(int itemIndex)
        {
            ItemId = itemIndex;
        }

        public void Dispose()
        {

        }
    }
}
