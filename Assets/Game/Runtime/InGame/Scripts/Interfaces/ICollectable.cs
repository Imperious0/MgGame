using Game.Runtime.InGame.Models;
using UnityEngine;

namespace Game.Runtime.InGame.Scripts.Interfaces
{
    public interface ICollectable : IGameItem
    {
        public CollectableId CollectableId { get; }
        public Transform CollectableTransform { get; }
    }
}
