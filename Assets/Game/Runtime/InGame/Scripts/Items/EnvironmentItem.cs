using Game.Runtime.InGame.Models;
using Game.Runtime.InGame.Scripts.Interfaces;
using Game.Runtime.InitializeHelper;
using UnityEngine;

namespace Game.Runtime.Scripts.Items
{
    public class EnvironmentItem : MonoBehaviour, IEnvironment
    {
        [SerializeField] private EnvironmentId _environmentId;
        public int ItemId { get; private set; }
        public EnvironmentId EnvironmentId => _environmentId;
        public Transform EnvironmentTransform => transform;
        public void Initialize(int itemIndex)
        {
            ItemId = itemIndex;
        }
    }
}
