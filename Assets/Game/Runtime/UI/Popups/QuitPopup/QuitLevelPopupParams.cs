using Game.Runtime.Models;
using Game.Runtime.UI.PopupBase;
using System;

namespace Game.Runtime.Scripts.UI.QuitLevel
{
    internal class QuitLevelPopupParams : PopupParams
    {
        public Action OnContinueGame { get; private set; }
        internal QuitLevelPopupParams(Action onContinueGame = null) : base(contentName: UIContentNames.QuitLevelPopup)
        {
            OnContinueGame = onContinueGame;
        }
    }
}
