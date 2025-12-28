using Game.AotHelper;
using Game.Runtime.InGame.Scripts.Models;
using Game.Runtime.PlayerData.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime.PlayerData
{
    public class Currencies
    {
        [Preserve(typeof(AotEnsureDictionary<CurrencyType, int>))]
        [JsonProperty("Currencies")]
        private Dictionary<CurrencyType, int> _currencies;

        [Preserve(typeof(AotEnsureDictionary<CurrencyType, long>))]
        [JsonProperty("CurrencyRegenData")]
        private Dictionary<CurrencyType, long> _currencyRegenData;

        public event Action OnCurrenciesUpdatedEvent;

        [JsonConstructor]
        private Currencies(Dictionary<CurrencyType, int> currencies,
            Dictionary<CurrencyType, long> currencyRegenData)
        {
            _currencies = currencies;
            _currencyRegenData = currencyRegenData;

            UpdateCurrencyRegens();
        }

        public bool TrySpendCurrency(CurrencyType currencyType, int spendAmount)
        {
            if(!_currencies.TryGetValue(currencyType, out int currencyAmount))
            {
                Debug.LogWarning($"Cant spend currency {currencyType.ToString()} that not exists in Currencies");
                return false;
            }

            if (spendAmount > currencyAmount) return false;

            _currencies[currencyType] -= spendAmount;

            TryStartCurrencyRegen(currencyType, DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            OnCurrenciesUpdatedEvent?.Invoke();

            return true;
        }

        public void EarnCurrency(CurrencyType currencyType, int earnAmount)
        {
            if (!_currencies.TryAdd(currencyType, earnAmount)) _currencies[currencyType] += earnAmount;
            OnCurrenciesUpdatedEvent?.Invoke();
        }

        public bool HasEnoughCurrency(CurrencyType currencyType, int amount)
        {
            if (!_currencies.TryGetValue(currencyType, out int currencyAmount)) return false;
            return amount <= _currencies[currencyType];
        }

        public bool TryGetCurrencyAmount(CurrencyType currencyType, out int currencyAmount)
        {
            return _currencies.TryGetValue(currencyType, out currencyAmount);
        }

        public bool TryStartCurrencyRegen(CurrencyType currencyType, long dateTimeUnixSecond)
        {
            return _currencyRegenData.TryAdd(currencyType, dateTimeUnixSecond);
        }

        public bool TryGetCurrencyRegen(CurrencyType currencyType, out long regenDate)
        {
            return _currencyRegenData.TryGetValue(currencyType, out regenDate);
        }

        public void UpdateCurrencyRegens()
        {
            List<CurrencyType> currencyRegenRemove = new List<CurrencyType>();
            List<CurrencyType> currencyRegenKeys = new List<CurrencyType>(_currencyRegenData.Keys);

            foreach (var currencyType in currencyRegenKeys)
            {
                if (!GameConstants.Currencies.TryGetCurrencyRegenDurations(currencyType, out long currencyRegenDuration))
                {
                    _currencyRegenData.Remove(currencyType);
                    continue;
                }
                long difference = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - _currencyRegenData[currencyType];

                if (difference < currencyRegenDuration) continue;

                long addAmount = difference / currencyRegenDuration;

                if (!_currencies.TryAdd(currencyType, (int)addAmount))
                {
                    _currencies[currencyType] += (int)addAmount;
                }

                _currencyRegenData[currencyType] = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - difference;

                if (GameConstants.Currencies.TryGetCurrencyMaxLimits(currencyType, out int currencyMaxLimits))
                {
                    if (_currencies[currencyType] >= currencyMaxLimits)
                    {
                        _currencies[currencyType] = currencyMaxLimits;
                        _currencyRegenData.Remove(currencyType);
                    }
                }
            }
        }

        public static Currencies CreateDefault() => new Currencies(new Dictionary<CurrencyType, int>() { { CurrencyType.Coin, 500 }, { CurrencyType.Energy, 5 } },
                                                                    new Dictionary<CurrencyType, long>());
    }
}
