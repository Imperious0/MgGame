using Game.Runtime.Models;
using Game.Runtime.UI.PopupBase;
using System;

namespace Game.Runtime.Scripts.UI.LevelSuccess
{
    internal class LevelSuccessPopupParams : PopupParams
    {
        internal LevelSuccessPopupParams() : base(contentName: UIContentNames.LevelSuccessPopup)
        {
        }
    }
}
