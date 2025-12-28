using Game.Runtime.InGame.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

namespace Game.Runtime.InGame.Scripts.Controller
{
    internal class UpdateHandler
    {
        private readonly List<IUpdatable> _updatables = new List<IUpdatable>();
        private bool _isInitialized;

        public UpdateHandler() { }

        public void Initialize()
        {
            if (_isInitialized) return;

            PlayerLoopSystem loop = PlayerLoop.GetCurrentPlayerLoop();
            PlayerLoopSystem loopSystem = new PlayerLoopSystem
            {
                type = typeof(UpdateHandler),
                updateDelegate = Tick
            };

            if (InsertCustomSystem(ref loop, typeof(UnityEngine.PlayerLoop.Update), loopSystem))
            {
                PlayerLoop.SetPlayerLoop(loop);
                _isInitialized = true;
            }
        }

        public void Dispose()
        {
            if (!_isInitialized) return;

            PlayerLoopSystem rootLoop = PlayerLoop.GetCurrentPlayerLoop();

            if (RemoveCustomSystem(ref rootLoop, typeof(UpdateHandler)))
            {
                PlayerLoop.SetPlayerLoop(rootLoop);
                _isInitialized = false;
            }

            _updatables.Clear();
        }

        private void Tick()
        {
            float dt = Time.deltaTime;
            float unscaledDt = Time.unscaledDeltaTime;
            for (int i = _updatables.Count - 1; i >= 0; i--)
            {
                _updatables[i].Tick(dt, unscaledDt);
            }
        }

        public void Register(IUpdatable item) => _updatables.Add(item);
        public void Unregister(IUpdatable item) => _updatables.Remove(item);

        private bool InsertCustomSystem(ref PlayerLoopSystem rootLoop, Type targetType, PlayerLoopSystem customSystem)
        {
            if (rootLoop.type == targetType)
            {
                var subSystems = rootLoop.subSystemList != null
                    ? new List<PlayerLoopSystem>(rootLoop.subSystemList)
                    : new List<PlayerLoopSystem>();

                subSystems.Add(customSystem);

                rootLoop.subSystemList = subSystems.ToArray();
                return true;
            }

            if (rootLoop.subSystemList != null)
            {
                for (int i = 0; i < rootLoop.subSystemList.Length; i++)
                {
                    if (InsertCustomSystem(ref rootLoop.subSystemList[i], targetType, customSystem))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool RemoveCustomSystem(ref PlayerLoopSystem rootLoop, Type targetType)
        {
            if (rootLoop.subSystemList == null) return false;

            var subSystems = new List<PlayerLoopSystem>(rootLoop.subSystemList);
            bool modified = false;

            for (int i = subSystems.Count - 1; i >= 0; i--)
            {
                if (subSystems[i].type == targetType)
                {
                    subSystems.RemoveAt(i);
                    modified = true;
                }
                else
                {
                    PlayerLoopSystem subLoop = subSystems[i];
                    if (RemoveCustomSystem(ref subLoop, targetType))
                    {
                        subSystems[i] = subLoop;
                        modified = true;
                    }
                }
            }

            if (modified) rootLoop.subSystemList = subSystems.ToArray();
            return modified;
        }
    }
}
