using UnityEngine;

namespace Game.SingletonHelper
{
    public class SingletonBehaviour<T> : MonoBehaviour where T : class
    {
        private static T _instance;
        public static T Instance { get => _instance; }

        protected void Awake()
        {
            if (_instance != null) { Destroy(this); return; }
            _instance = this as T;
        }
        protected abstract void OnAwake();

    }
}
