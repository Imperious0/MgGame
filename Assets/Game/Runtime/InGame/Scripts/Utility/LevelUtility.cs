using Game.Runtime.InGame.Models.Level;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Game.Runtime.InGame.Scripts.Utility
{
    internal static class LevelUtility
    {
        public static string[] GetLevels()
        {
            TextAsset[] textAssets = Resources.LoadAll<TextAsset>("Levels");
            if (textAssets == null || textAssets.Length == 0)
            {
                throw new FileNotFoundException($"Cant find any of Levels!");
            }

            string[] levels = textAssets.Select(t => t.name).ToArray();

            return levels;
        }
        public static LevelData GetLevel(int level)
        {
            string[] levels = GetLevels();
            int totalLevels = levels.Length;

            level %= totalLevels;

            TextAsset actualLevel = Resources.Load<TextAsset>($"LevelData/{levels[level]}");

            LevelData levelData = JsonConvert.DeserializeObject<LevelData>(actualLevel.text);

            return levelData;
        }
    }
}
