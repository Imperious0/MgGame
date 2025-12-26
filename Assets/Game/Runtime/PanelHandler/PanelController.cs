using System;
using System.Collections.Generic;
using Game.Runtime.InitializeHelper;
using Game.Runtime.UI.PanelBase;
using Game.Runtime.UI.PanelBase.Helper;
using Game.SingletonHelper;
using UnityEngine;

namespace Game.Runtime.PanelHandler
{
    [Serializable]
    internal struct PanelCatalog
    {
        public string PanelName;
        public PanelBase PanelContent;
    }

    public class PanelController : SingletonBehaviour<PanelController>, IInitializable
    {
        [SerializeField] private List<PanelCatalog> _panelLists;
        [SerializeField] private ScrollingHandler _scrollingHandler;
        
        protected override void OnAwake()
        {
            InitializeController.Instance.RegisterInitialize(this);
        }

        public void Initialize()
        {
            foreach (var panel in _panelLists)
            {
                panel.PanelContent.Initialize();
            }
            _scrollingHandler?.Initialize();
        }
    }
}
