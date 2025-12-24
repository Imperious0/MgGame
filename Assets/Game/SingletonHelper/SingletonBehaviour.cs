using UnityEngine;

namespace Game.SingletonHelper
{
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        public static T Instance {
            get
            {
                if (!_instance)
                {
                    _instance = (T)FindFirstObjectByType(typeof(T));
                    if (!_instance)
                    {
                        Debug.LogWarning($"Cant find {typeof(T)} in Scene");
                    }
                    else
                    {
                        SingletonBehaviour<T> instance = _instance as SingletonBehaviour<T>;
                        instance.OnAwake();
                    }
                }

                return _instance;
            }
        }

        protected void Awake()
        {
            if (_instance != null) { Destroy(this); return; }
            _instance = this as T;

            OnAwake();
        }
        protected abstract void OnAwake();

    }
}
