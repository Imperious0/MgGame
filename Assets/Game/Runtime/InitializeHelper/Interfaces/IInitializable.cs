using UnityEngine;

namespace Game.Runtime.InitializeHelper
{
    public interface IInitializable
    {
        public bool MarkDisposeOnSceneChange { get; }
        public void Initialize();
        public void Dispose();
    }
}
