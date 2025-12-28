using Game.Runtime.Bootstrapper;
using Game.Runtime.InGame.Scripts.Models;
using Game.Runtime.Models;
using Game.Runtime.PlayerData;
using Game.Runtime.PlayerData.Models;
using Game.Runtime.UI.MainMenu.Currencies;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Runtime.UI.MainMenu
{
    public class MainMenuPanel : PanelBase.PanelBase
    {
        [SerializeField] private CurrencyView _currencyView;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Button _playButton;
        protected override void OnInitialize()
        {
            _levelText.text = $"LEVEL {PlayerDataController.Instance.AccountData.GameLevel}";
            _playButton.onClick.AddListener(StartGame);
            _currencyView.Initialize();
        }
        protected override void OnDispose()
        {
            _playButton.onClick.RemoveListener(StartGame);
            _currencyView.Dispose();
        }

        private void StartGame()
        {
            if(PlayerDataController.Instance.Currencies.TrySpendCurrency(CurrencyType.Energy, GameConstants.GameSettings.LevelEnergyAmount))
            {
                PlayerDataController.Instance.SaveCurrencies();
                GameController.Instance.LoadScene(SceneNames.GamePlayScene);
            }
        }

        private void Update()
        {
            PlayerDataController.Instance.Currencies.UpdateCurrencyRegens();
        }
    }
}
