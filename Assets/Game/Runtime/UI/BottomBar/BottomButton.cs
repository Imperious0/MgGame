using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Runtime.UI.BottomBar
{
    public class BottomButton : MonoBehaviour
    {
        [SerializeField] private string UIContentName;

        [SerializeField] private Button _button;

        public event Action<string> _onButtonClicked;

        public void Initialize(Action<string> onButtonClicked)
        {
            _onButtonClicked = onButtonClicked;
            _button.onClick.AddListener(OnButtonClicked);
        }

        public void Dispose()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            _onButtonClicked?.Invoke(UIContentName);
        }
    }
}