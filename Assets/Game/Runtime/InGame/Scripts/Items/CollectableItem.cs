using Game.Runtime.InGame.Models;
using Game.Runtime.InGame.Scripts.Interfaces;
using UnityEngine;

namespace Game.Runtime.Scripts.Items
{
    public class CollectableItem : MonoBehaviour, ICollectable
    {
        [SerializeField] private CollectableId _collectableId;
        public CollectableId CollectableId => _collectableId;
        public Transform CollectableTransform => transform;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
