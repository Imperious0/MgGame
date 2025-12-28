using Game.Runtime.Bootstrapper;
using Game.Runtime.Models;
using Game.Runtime.PlayerData;
using Game.Runtime.PlayerData.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Runtime.UI.MainMenu
{
    public class MainMenuPanel : PanelBase.PanelBase
    {
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Button _playButton;
        protected override void OnInitialize()
        {
            _levelText.text = $"LEVEL {PlayerDataController.Instance.AccountData.GameLevel}";
            _playButton.onClick.AddListener(StartGame);
        }
        protected override void OnDispose()
        {
            _playButton.onClick.RemoveListener(StartGame);
        }

        private void StartGame()
        {
            if(PlayerDataController.Instance.Currencies.TrySpendCurrency(CurrencyType.Energy, 1))
            {
                PlayerDataController.Instance.SaveCurrencies();
                GameController.Instance.LoadScene(SceneNames.GamePlayScene);
            }
        }
    }
}
