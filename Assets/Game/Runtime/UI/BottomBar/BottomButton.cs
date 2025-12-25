using System;
using UnityEngine;
using UnityEngine.UIElements;

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
            _button.RegisterCallback<ClickEvent>(OnButtonClicked);
        }

        public void Dispose()
        {
            _button.UnregisterCallback<ClickEvent>(OnButtonClicked);
        }

        private void OnButtonClicked(ClickEvent evt)
        {
            _onButtonClicked?.Invoke(UIContentName);
        }
    }
}