using Game.Runtime.InGame.Models.Level;
using Game.Runtime.InGame.Scripts.Models;
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
            if (!Directory.Exists(GameConstants.LevelsFolder))
            {
                throw new FileNotFoundException($"Cant find any of Levels!");
            }

            string[] levels = Directory.GetFiles(GameConstants.LevelsFolder, "*.json").Select(Path.GetFileNameWithoutExtension).ToArray();

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
