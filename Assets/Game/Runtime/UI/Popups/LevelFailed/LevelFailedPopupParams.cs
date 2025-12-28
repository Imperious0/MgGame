using Game.Runtime.Models;
using Game.Runtime.UI.PopupBase;
using System;

namespace Game.Runtime.Scripts.UI.LevelFailed
{
    internal class LevelFailedPopupParams : PopupParams
    {
        internal LevelFailedPopupParams() : base(contentName: UIContentNames.LevelFailedPopup)
        {
        }
    }
}
