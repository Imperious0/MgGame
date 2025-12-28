using Game.Runtime.Models;
using Game.Runtime.UI.PopupBase;
using System;

namespace Game.Runtime.Scripts.UI.Pause
{
    internal class PausePopupParams : PopupParams
    {
        public Action OnContinueGame { get; private set; }
        internal PausePopupParams(Action onContinueGame = null) : base(contentName: UIContentNames.PausePopup)
        {
            OnContinueGame = onContinueGame;
        }
    }
}
