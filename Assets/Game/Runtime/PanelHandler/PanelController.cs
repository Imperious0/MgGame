using System;
using System.Collections.Generic;
using DG.Tweening;
using Game.Runtime.InitializeHelper;
using Game.Runtime.UI.PanelBase;
using Game.Runtime.UI.PanelBase.Helper;
using Game.Runtime.UI.PopupBase;
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

    [Serializable]
    internal struct PopupCatalog
    {
        public string PopupName;
        public PopupBase PopupContent;
    }

    public class PanelController : SingletonBehaviour<PanelController>, IInitializable
    {
        [SerializeField] private List<PanelCatalog> _panelLists;
        [SerializeField] private List<PopupCatalog> _popupList;
        [SerializeField] private ScrollingHandler _scrollingHandler;

        public bool MarkDisposeOnSceneChange => true;

        private PopupBase _activePopup;

        private const float PopupAnimationDuration = 0.3f;

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

        public void Dispose()
        {
            foreach (var panel in _panelLists)
            {
                panel.PanelContent.Dispose();
            }

            _scrollingHandler?.Dispose();
        }

        internal void OpenPopup<T>(T popupParams, bool isInstant = false) where T : PopupParams
        {
            if (_activePopup != null)
            {
                Debug.LogWarning($"[{popupParams.ContentName}] There is active popup use ChangePopup instead!");
                return;
            }
            PopupBase<T> activePopup = null;
            foreach (var popupCatalog in _popupList)
            {
                if(string.Equals(popupCatalog.PopupName, popupParams.ContentName, StringComparison.InvariantCulture))
                {
                    activePopup = popupCatalog.PopupContent as PopupBase<T>;
                    break;
                }
            }

            if(activePopup == null)
            {
                Debug.LogWarning($"There is no popup with ContentName: {popupParams.ContentName}");
                return;
            }

            activePopup.DOKill(true);
            _activePopup = activePopup;
            activePopup.ContentRoot.localScale = Vector3.zero;
            activePopup.gameObject.SetActive(true);

            activePopup.ContentRoot.DOScale(Vector3.one, isInstant ? 0f : PopupAnimationDuration).OnComplete(() =>
            {
                activePopup.Open(popupParams);
            });

        }

        internal void ChangePopup<T>(T popupParams, bool isInstant = false) where T : PopupParams
        {
            if(_activePopup == null)
            {
                Debug.LogWarning($"[{popupParams.ContentName}] There is no active popup use OpenPopup instead!");
                OpenPopup(popupParams, isInstant);
                return;
            }

            ClosePopup(isInstant: isInstant, () => OpenPopup(popupParams, isInstant));
        }

        public void ClosePopup(bool isInstant = false, Action onClosed = null)
        {
            if(_activePopup == null)
            {
                Debug.LogWarning($"Cant call ClosePopup even there is no ActivePopup!");
                return;
            }

            _activePopup.DOKill(true);
            _activePopup.ContentRoot.DOScale(Vector3.zero, isInstant ? 0f: PopupAnimationDuration).OnComplete(() => 
            {
                _activePopup.Close();
                _activePopup.gameObject.SetActive(false);
                _activePopup = null;
                onClosed?.Invoke();
            });
        }
    }
}
