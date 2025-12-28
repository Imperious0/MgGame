using Game.Runtime.Bootstrapper;
using Game.Runtime.Models;
using Game.Runtime.Scripts.UI.LevelSuccess;
using Game.Runtime.UI.PopupBase;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Runtime.Scripts.UI.Popups.LevelSuccess
{
    internal class LevelSuccessPopup : PopupBase<LevelSuccessPopupParams>
    {
        [SerializeField] private Button _continueButton;

        protected override void OnOpen(LevelSuccessPopupParams popupParams)
        {
            _continueButton.onClick.AddListener(OnContinueNextLevel);
        }

        protected override void OnClose()
        {
            _continueButton.onClick.RemoveListener(OnContinueNextLevel);
        }

        private void OnContinueNextLevel() 
        {
            _continueButton.onClick.RemoveListener(OnContinueNextLevel);

            GameController.Instance.LoadScene(SceneNames.GamePlayScene);
        }
    }
}
