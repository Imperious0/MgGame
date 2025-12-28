using Game.Runtime.InGame.Scripts.Interfaces;
using Game.Runtime.InitializeHelper;
using System;

namespace Game.Runtime.InGame.Scripts.Controller
{
    internal class GameDurationHandler : IUpdatable
    {
        private float _gameDuration;

        private bool _activeTicking;
        private bool _timeOver;

        private Action _onTimeOverEvent;

        public GameDurationHandler(float gameDuration, Action onTimeOverEvent = null)
        {
            _gameDuration = gameDuration;

            _onTimeOverEvent = onTimeOverEvent;

            _activeTicking = false;

            _timeOver = false;
        }

        public void Initialize()
        {
            _activeTicking = false;
            _timeOver = false;
        }

        public void Dispose()
        {
        }

        public void Tick(float deltaTime, float unscaledDeltaTime)
        {
            if (!_activeTicking) return;

            if (_gameDuration <= 0f)
            {
                if (!_timeOver)
                {
                    _timeOver = true;
                    _activeTicking = false;
                    _onTimeOverEvent?.Invoke();
                }
                return;
            }
            _gameDuration -= deltaTime;
        }

        public bool IsDurationOver => _gameDuration <= 0f;

        public void StartTick()
        {
            _activeTicking = true;
        }

        public void StopTick() 
        {
            _activeTicking = false;
        }

    }
}
