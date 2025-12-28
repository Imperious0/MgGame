using Game.Runtime.Bootstrapper;
using Game.Runtime.InGame.Scripts.Models;
using Game.Runtime.Models;
using Game.Runtime.PlayerData;
using Game.Runtime.PlayerData.Models;
using Game.Runtime.Scripts.UI.LevelFailed;
using Game.Runtime.UI.PopupBase;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Runtime.Scripts.UI.Popups.LevelFailed
{
    internal class LevelFailedPopup : PopupBase<LevelFailedPopupParams>
    {
        [SerializeField] private Button _closePopup;

        [SerializeField] private Button _tryAgainButton;
        [SerializeField] private TextMeshProUGUI _tryAgainButtonText;

        protected override void OnOpen(LevelFailedPopupParams popupParams)
        {
            _tryAgainButton.onClick.AddListener(OnTryAgain);
            _closePopup.onClick.AddListener(OnLeaveGame);

            _tryAgainButtonText.text = PlayerDataController.Instance.Currencies.HasEnoughCurrency(CurrencyType.Energy, GameConstants.GameSettings.LevelEnergyAmount) ? "Try Again" : "Main Menu";
        }

        protected override void OnClose()
        {
            _tryAgainButton.onClick.RemoveListener(OnTryAgain);
            _closePopup.onClick.RemoveListener(OnLeaveGame);
        }

        private void OnLeaveGame()
        {
            _closePopup.onClick.RemoveListener(OnLeaveGame);
            GameController.Instance.LoadScene(SceneNames.MainMenuScene);
        }
        private void OnTryAgain() 
        {
            _tryAgainButton.onClick.RemoveListener(OnTryAgain);

            if (PlayerDataController.Instance.Currencies.HasEnoughCurrency(CurrencyType.Energy, GameConstants.GameSettings.LevelEnergyAmount))
            {
                if(PlayerDataController.Instance.Currencies.TrySpendCurrency(CurrencyType.Energy, GameConstants.GameSettings.LevelEnergyAmount))
                {
                    PlayerDataController.Instance.SaveCurrencies();
                    GameController.Instance.LoadScene(SceneNames.GamePlayScene);
                }
                else
                {
                    GameController.Instance.LoadScene(SceneNames.MainMenuScene);
                }
            }
            else
            {
                GameController.Instance.LoadScene(SceneNames.MainMenuScene);
            }
        }
    }
}
