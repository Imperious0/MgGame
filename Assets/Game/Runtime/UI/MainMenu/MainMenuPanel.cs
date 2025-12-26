using Game.Runtime.Bootstrapper;
using Game.Runtime.Models;
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
            _playButton.onClick.AddListener(StartGame);
        }
        protected override void OnDispose()
        {
            _playButton.onClick.RemoveListener(StartGame);
        }

        private void StartGame()
        {
            GameController.Instance.LoadScene(SceneNames.GamePlayScene);
        }
    }
}
