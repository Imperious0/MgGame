using Game.Runtime.InGame.Scripts.Interfaces;
using Game.Runtime.InitializeHelper;
using Game.Runtime.Scripts;
using System;

namespace Game.Runtime.InGame.Scripts
{
    internal class GameDurationHalder : IInitializable, IUpdatable
    {
        private float _gameDuration;

        private bool _timeOver;

        private Action _onTimeOverEvent;

        public GameDurationHalder(float gameDuration, Action onTimeOverEvent = null)
        {
            _gameDuration = gameDuration;

            _onTimeOverEvent = onTimeOverEvent;

            _timeOver = false;
        }

        public void Initialize()
        {
            InGameController.Instance.GameUpdateHandler.Register(this);
            _timeOver = false;
        }

        public void Dispose()
        {
            InGameController.Instance.GameUpdateHandler.Unregister(this);
        }

        public void Tick(float deltaTime, float unscaledDeltaTime)
        {
            if (_gameDuration <= 0f)
            {
                if (!_timeOver)
                {
                    _timeOver = true;
                    _onTimeOverEvent?.Invoke();
                }
                return;
            }
            _gameDuration -= deltaTime;
        }

        public bool IsDurationOver => _gameDuration <= 0f;
    }
}
