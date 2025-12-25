using System;
using System.Collections.Generic;
using Game.Runtime.UI.BottomBar;
using UnityEngine;

namespace Game.Runtime.UI.PanelBase.Helper
{
    public class ScrollingHandler : MonoBehaviour
    {
        [SerializeField] private List<BottomButton> _bottomButtons;

        public void Initialize()
        {
            foreach (var button in _bottomButtons)
            {
                button.Initialize(OnButtonClicked);
            }
        }

        public void Dispose()
        {
            foreach (var button in _bottomButtons)
            {
                button.Dispose();
            }
        }

        private void OnButtonClicked(string contentName)
        {
        }
    }
}