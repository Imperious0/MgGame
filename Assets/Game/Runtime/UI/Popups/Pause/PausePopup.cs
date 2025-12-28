using Game.Runtime.PanelHandler;
using Game.Runtime.PlayerData;
using Game.Runtime.Scripts.UI.Pause;
using Game.Runtime.Scripts.UI.QuitLevel;
using Game.Runtime.UI.PopupBase;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Runtime.Scripts.UI.Popups.Pause
{
    internal class PausePopup : PopupBase<PausePopupParams>
    {
        [SerializeField] private Button _closePopup;

        [SerializeField] private Button _leaveGameButton;
        [SerializeField] private Button _continueGame;

        [SerializeField] private Button _soundButton;
        [SerializeField] private Button _musicButton;
        [SerializeField] private Button _vibrationButton;

        protected override void OnOpen(PausePopupParams popupParams)
        {
            _leaveGameButton.onClick.AddListener(OnLeaveGame);
            _continueGame.onClick.AddListener(OnContinueGame);
            _closePopup.onClick.AddListener(OnContinueGame);

            _soundButton.onClick.AddListener(OnSoundClicked);
            _musicButton.onClick.AddListener(OnMusicClicked);
            _vibrationButton.onClick.AddListener(OnVibrationClicked);

            UpdateSoundButtonState();
            UpdateMusicButtonState();
            UpdateVibrationButtonState();
        }

        protected override void OnClose()
        {
            _leaveGameButton.onClick.RemoveListener(OnLeaveGame);
            _continueGame.onClick.RemoveListener(OnLeaveGame);
            _closePopup.onClick.RemoveListener(OnLeaveGame);

            _soundButton.onClick.RemoveListener(OnSoundClicked);
            _musicButton.onClick.RemoveListener(OnMusicClicked);
            _vibrationButton.onClick.RemoveListener(OnVibrationClicked);
        }

        private void OnLeaveGame()
        {
            _leaveGameButton.onClick.RemoveListener(OnLeaveGame);
            PanelController.Instance.ChangePopup(new QuitLevelPopupParams(Params.OnContinueGame));
        }
        private void OnContinueGame() 
        {
            _continueGame.onClick.RemoveListener(OnContinueGame);
            _closePopup.onClick.RemoveListener(OnContinueGame);
            Params.OnContinueGame?.Invoke();
        }

        private void OnSoundClicked()
        {
            PlayerDataController.Instance.AccountData.SoundState = !PlayerDataController.Instance.AccountData.SoundState;
            UpdateSoundButtonState();
            PlayerDataController.Instance.SaveAccountData();
        }
        private void UpdateSoundButtonState()
        {
            _soundButton.image.color = PlayerDataController.Instance.AccountData.SoundState ? Color.green : Color.red;
        }
        private void OnMusicClicked()
        {
            PlayerDataController.Instance.AccountData.MusicState = !PlayerDataController.Instance.AccountData.MusicState;
            UpdateMusicButtonState();
            PlayerDataController.Instance.SaveAccountData();
        }
        private void UpdateMusicButtonState()
        {
            _musicButton.image.color = PlayerDataController.Instance.AccountData.MusicState ? Color.green : Color.red;
        }
        private void OnVibrationClicked()
        {
            PlayerDataController.Instance.AccountData.VibrationState = !PlayerDataController.Instance.AccountData.VibrationState;
            UpdateVibrationButtonState();
            PlayerDataController.Instance.SaveAccountData();
        }
        private void UpdateVibrationButtonState()
        {
            _vibrationButton.image.color = PlayerDataController.Instance.AccountData.VibrationState ? Color.green : Color.red;
        }
    }
}
