using Game.Runtime.PlayerData.Models;
using System.Collections.Generic;

namespace Game.Runtime.InGame.Scripts.Models
{
    public static class GameConstants
    {
        public static class GameSettings
        {
            public const int LevelEnergyAmount = 1;
            public const int MaxEnergyAmount = 5;

            public const int MatchItemCount = 3;
            public const int EmptySlotCount = 6;
        }

        public static class Currencies
        {
            private static IReadOnlyDictionary<CurrencyType, long> _currencyRegenDurations = new Dictionary<CurrencyType, long>()
            {
                { CurrencyType.Energy, 1800 }, //30 min
            };

            public static bool TryGetCurrencyRegenDurations(CurrencyType currencyType, out long currencyRegenDuration)
            {
                return _currencyRegenDurations.TryGetValue(currencyType, out currencyRegenDuration);
            }

            private static IReadOnlyDictionary<CurrencyType, int> _currencyMaxLimits = new Dictionary<CurrencyType, int>()
            {
                { CurrencyType.Energy, GameSettings.MaxEnergyAmount },
            };
            public static bool TryGetCurrencyMaxLimits(CurrencyType currencyType, out int currencyMaxLimits)
            {
                return _currencyMaxLimits.TryGetValue(currencyType, out currencyMaxLimits);
            }
        }
    }
}
