using UnityEngine;

namespace Game.Runtime.UI.PanelBase
{
    public abstract class PanelBase : MonoBehaviour
    {
        public void Initialize()
        {
            OnInitialize();
        }

        public void Dispose()
        {
            OnDispose();
        }

        protected abstract void OnInitialize();
        protected abstract void OnDispose();

    }
}