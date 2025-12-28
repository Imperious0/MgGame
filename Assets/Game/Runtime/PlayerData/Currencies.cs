using Game.AotHelper;
using Game.Runtime.PlayerData.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime.PlayerData
{
    public class Currencies
    {
        [Preserve(typeof(AotEnsureDictionary<CurrencyType, int>))]
        [JsonProperty("Currencies")]
        private Dictionary<CurrencyType, int> _currencies;

        [JsonConstructor]
        private Currencies(Dictionary<CurrencyType, int> currencies )
        {
            _currencies = currencies;
        }

        public bool TrySpendCurrency(CurrencyType currencyType, int spendAmount)
        {
            if(!_currencies.TryGetValue(currencyType, out int currencyAmount))
            {
                Debug.LogWarning($"Cant spend currency {currencyType.ToString()} that not exists in Currencies");
                return false;
            }

            if (spendAmount >= currencyAmount) _currencies[currencyType] = 0;
            else _currencies[currencyType] -= spendAmount;

            return true;
        }

        public void EarnCurrency(CurrencyType currencyType, int earnAmount)
        {
            if (!_currencies.TryAdd(currencyType, earnAmount)) _currencies[currencyType] += earnAmount;
        }

        public bool HasEnoughCurrency(CurrencyType currencyType, int amount)
        {
            if (!_currencies.TryGetValue(currencyType, out int currencyAmount)) return false;
            return amount >= _currencies[currencyType];
        }

        public static Currencies CreateDefault() => new Currencies(new Dictionary<CurrencyType, int>() { { CurrencyType.Coin, 500 }, { CurrencyType.Energy, 5 } });
    }
}
