using System;
using System.Collections.Generic;
using Game.SingletonHelper;
using UnityEngine;

namespace Game.PanelHandler
{
    [Serializable]
    internal struct PanelCatalog
    {
        public string PanelName;
        public PanelBase PanelContent;
    }

    public class PanelController : SingletonBehaviour<PanelController>
    {
        [SerializeField] private List<PanelCatalog> _panelLists;
        [SerializeField] private ScrollingHandler _scrollingHandler;
        
        protected override void OnAwake()
        {
            foreach (var panel in _panelLists)
            {
                panel.PanelContent.Initialize();
            }
        }
    }
}
