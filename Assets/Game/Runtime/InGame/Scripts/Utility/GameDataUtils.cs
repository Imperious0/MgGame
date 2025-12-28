using Game.Runtime.InGame.Models.Level;
using UnityEngine;

namespace Game.Runtime.InGame.Scripts.Utility
{
    public static class GameDataUtils
    {
        public static void ApplyDataToTransform(Transform target, GameItemData data)
        {
            target.position = data.Position;
            target.eulerAngles = data.Rotation;
            target.localScale = data.Scale;
        }
    }
}
