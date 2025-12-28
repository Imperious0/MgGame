using Game.Runtime.PanelHandler;
using Game.Runtime.Scripts.UI.LevelFailed;
using Game.Runtime.Scripts.UI.QuitLevel;
using Game.Runtime.UI.PopupBase;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Runtime.Scripts.UI.Popups.QuitLevel
{
    internal class QuitLevelPopup : PopupBase<QuitLevelPopupParams>
    {
        [SerializeField] private Button _closePopup;

        [SerializeField] private Button _leaveGameButton;
        [SerializeField] private Button _continueGame;

        protected override void OnOpen(QuitLevelPopupParams popupParams)
        {
            _leaveGameButton.onClick.AddListener(OnLeaveGame);
            _continueGame.onClick.AddListener(OnContinueGame);
            _closePopup.onClick.AddListener(OnContinueGame);
        }

        protected override void OnClose()
        {
            _leaveGameButton.onClick.RemoveListener(OnLeaveGame);
            _continueGame.onClick.RemoveListener(OnLeaveGame);
            _closePopup.onClick.RemoveListener(OnLeaveGame);
        }

        private void OnLeaveGame()
        {
            _leaveGameButton.onClick.RemoveListener(OnLeaveGame);
            PanelController.Instance.ChangePopup(new LevelFailedPopupParams());
        }
        private void OnContinueGame() 
        {
            _continueGame.onClick.RemoveListener(OnContinueGame);
            _closePopup.onClick.RemoveListener(OnContinueGame);
            Params.OnContinueGame?.Invoke();
        }
    }
}
