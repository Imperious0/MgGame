using Game.Runtime.Bootstrapper;
using Game.Runtime.PlayerData;
using Game.Runtime.PlayerData.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Runtime.UI.MainMenu.Currencies
{
    public class CurrencyView : MonoBehaviour
    {
        [SerializeField] private CurrencyType _currencyType;
        [SerializeField] private Image _currencyImage;
        [SerializeField] private TextMeshProUGUI _currencyAmount;

        [SerializeField] private CurrencyRegenTracker _regenTracker;

        public void Initialize()
        {
            _currencyImage.sprite = GameController.Instance.PrefabDatabase.GetCurrencySprite(_currencyType);
            _currencyAmount.text = $"{(PlayerDataController.Instance.Currencies.TryGetCurrencyAmount(_currencyType, out int currencyAmount) ? currencyAmount : 0)}";

            PlayerDataController.Instance.Currencies.OnCurrenciesUpdatedEvent += OnCurrenciesUpdated;

            _regenTracker?.Initialize(_currencyType);
        }

        public void Dispose()
        {
            PlayerDataController.Instance.Currencies.OnCurrenciesUpdatedEvent -= OnCurrenciesUpdated;
            _regenTracker?.Dispose();
        }

        private void OnCurrenciesUpdated()
        {
            _currencyAmount.text = $"{(PlayerDataController.Instance.Currencies.TryGetCurrencyAmount(_currencyType, out int currencyAmount) ? currencyAmount : 0)}";
        }
    }
}
