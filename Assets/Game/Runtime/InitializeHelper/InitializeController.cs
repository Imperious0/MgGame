using Game.Runtime.Assets.Runtime.Models;
using Game.Runtime.Bootstrapper;
using Game.SingletonHelper;
using System;
using System.Collections.Generic;

namespace Game.Runtime.InitializeHelper
{
    public class InitializeController : SingletonBehaviour<InitializeController>
    {
        public bool InitializeCompleted { get; private set; }
        private Dictionary<int, IInitializable> _orderedPreInitializes = new Dictionary<int, IInitializable>();
        private Dictionary<int, IInitializable> _postInitializes = new Dictionary<int, IInitializable>();

        private IReadOnlyDictionary<Type, int> _sceneInitializeOrders;
        private int _postInitializeIndex;
        protected override void OnAwake()
        {
            _sceneInitializeOrders = null;
            _postInitializeIndex = 0;
        }

        protected void OnEnable()
        {
            Initialize();
            InitializeCompleted = true;
        }

        private void OnDestroy()
        {
            InitializeCompleted = false;
        }

        public void RegisterInitialize<T>(T initializable) where T : IInitializable
        {
            if (_sceneInitializeOrders == null) InitializeOrder.TryGetInitializeOrdersByChapter(GameController.Instance.ActiveSceneName, out _sceneInitializeOrders);

            if (_sceneInitializeOrders != null && _sceneInitializeOrders.TryGetValue(typeof(T), out int initializeIndex))
            {
                if (!_orderedPreInitializes.TryAdd(initializeIndex, initializable))
                    throw new ArgumentOutOfRangeException($"Cant add initializeable: {typeof(T)} to orderedPreInitializes index: {initializeIndex}, PreTaker: {_orderedPreInitializes[initializeIndex].GetType()}");
                return;
            }

            if (!_postInitializes.TryAdd(_postInitializeIndex, initializable))
                throw new ArgumentOutOfRangeException($"Cant add initializeable: {typeof(T)} to postInitializes index: {_postInitializeIndex}, PreTaker: {_postInitializes[_postInitializeIndex].GetType()}");
            _postInitializeIndex++;
        }

        private void Initialize()
        {
            foreach (var kvp in _orderedPreInitializes)
            {
                kvp.Value.Initialize();
            }

            foreach (var kvp in _postInitializes)
            {
                kvp.Value.Initialize();
            }
        }

    }
}
