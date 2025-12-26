using Game.SingletonHelper;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Runtime.UI.InputBlocker
{
    public class InputBlocker : SingletonBehaviour<InputBlocker>
    {
        [SerializeField] private Image _blocker;
        protected override void OnAwake()
        {

        }

        public void BlockInteractions()
        {
            _blocker.raycastTarget = true;
        }

        public void Release()
        {
            _blocker.raycastTarget = false;
        }
    }
}
