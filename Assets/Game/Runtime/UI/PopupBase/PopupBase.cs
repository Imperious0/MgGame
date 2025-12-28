using UnityEngine;

namespace Game.Runtime.UI.PopupBase
{
    internal abstract class PopupBase : MonoBehaviour
    {
        [SerializeField] private RectTransform _contentRoot;
        public RectTransform ContentRoot { get => _contentRoot; }
        public void Close()
        {
            OnClose();
        }

        protected abstract void OnClose();
    }

    internal abstract class PopupBase<T> : PopupBase where T : PopupParams
    {
        protected T Params;
        public void Open(T popupParams)
        {
            Params = popupParams;
            OnOpen(Params);
        }

        protected abstract void OnOpen(T popupParams);
    }
}
