using Game.Runtime.InGame.Models;
using Game.Runtime.InGame.Scripts.Interfaces;
using UnityEngine;

namespace Game.Runtime.Scripts.Items
{
    public class EnvironmentItem : MonoBehaviour, IEnvironment
    {
        [SerializeField] private EnvironmentId _environmentId;
        public EnvironmentId EnvironmentId => _environmentId;
        public Transform EnvironmentTransform => transform;

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
