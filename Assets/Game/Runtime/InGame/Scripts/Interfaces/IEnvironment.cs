using Game.Runtime.InGame.Models;
using UnityEngine;

namespace Game.Runtime.InGame.Scripts.Interfaces
{
    public interface IEnvironment
    {
        public EnvironmentId EnvironmentId { get; }
        public Transform EnvironmentTransform { get; }
    }
}
