using Game.Runtime.InGame.Scripts.Interfaces;
using Game.Runtime.InGame.Scripts.Models;
using Game.Runtime.PlayerData;
using Game.Runtime.PlayerData.Models;
using System;
using TMPro;
using UnityEngine;

namespace Game.Runtime.UI.MainMenu.Currencies
{
    public class CurrencyRegenTracker : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _countDownText;

        private CurrencyType _currencyType;

        private long _regenDuration;
        public void Initialize(CurrencyType currencyType)
        {
            _currencyType = currencyType;
            if(!GameConstants.Currencies.TryGetCurrencyRegenDurations(_currencyType, out _regenDuration))
            {
                Debug.LogWarning($"Cant find RegenDuration for currencyType {_currencyType.ToString()}");
            }
        }
        public void Dispose()
        {

        }

        private void Update()
        {
            if(!PlayerDataController.Instance.Currencies.TryGetCurrencyRegen(_currencyType, out long regenData) || regenData == 0)
            {
                _countDownText.text = $"Full";
                return;
            }

            long dtNow = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            long nextRegen = regenData + _regenDuration;
            long diff = nextRegen - dtNow;
            int m = Mathf.FloorToInt(diff / 60);
            int s = Mathf.FloorToInt(diff % 60);
            _countDownText.text = string.Format("{0:00}m {1:00}s", m, s);
        }
    }
}
