
using UnityEngine;

namespace Game.Runtime.InGame.Models.Level
{
    public readonly struct GameItemData
    {
        public readonly Vector3 Position;
        public readonly Vector3 Rotation;
        public readonly Vector3 Scale;

        public GameItemData(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            this.Position = position;
            this.Rotation = rotation;
            this.Scale = scale;
        }
    }
}
