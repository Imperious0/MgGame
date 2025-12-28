using DG.Tweening;
using Game.Runtime.InGame.Models;
using Game.Runtime.InGame.Scripts.Controller;
using Game.Runtime.InGame.Scripts.Interfaces;
using Game.Runtime.PanelHandler;
using Game.Runtime.Scripts.UI.Pause;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Runtime.UI.InGame
{
    public class InGamePanel : PanelBase.PanelBase, IUpdatable
    {
        [SerializeField] private Button _pauseButton;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _timerText;

        [SerializeField] private AwaitingRootView _awaitingRootViewPrefab;
        [SerializeField] private RectTransform _awaitingRoot;

        [SerializeField] private CollectableView _collectableViewPrefab;
        [SerializeField] private List<RectTransform> _collectedRoots;

        private List<AwaitingRootView> _awaitingRootViews = new List<AwaitingRootView>();
        private List<CollectableView> _collectableViews = new List<CollectableView>();


        protected override void OnInitialize()
        {
            _pauseButton.onClick.AddListener(OnPauseClicked);

            _levelText.text = $"LEVEL {InGameController.Instance.ActiveLevel}";

            CreateAwaitingRootViews();

            InGameController.Instance.CollectableHandler.OnItemCollectedEvent += OnCollectableCollected;

            InGameController.Instance.GameUpdateHandler.Register(this);
        }
        protected override void OnDispose()
        {
            _pauseButton.onClick.AddListener(OnPauseClicked);
            InGameController.Instance.GameUpdateHandler.Unregister(this);
            InGameController.Instance.CollectableHandler.OnItemCollectedEvent -= OnCollectableCollected;

            ClearAwaitingRootViews();
        }

        private void OnPauseClicked()
        {
            InGameController.Instance.GameDurationHandler.StopTick();
            PanelController.Instance.OpenPopup(new PausePopupParams(OnContinueGame));
        }

        private void OnContinueGame()
        {
            PanelController.Instance.ClosePopup();
            InGameController.Instance.GameDurationHandler.StartTick();
        }


        public void Tick(float dt, float udt)
        {
            _timerText.text = InGameController.Instance.GameDurationHandler.GetDurationText();
        }

        private void CreateAwaitingRootViews()
        {
            foreach (var kvp in InGameController.Instance.CollectableHandler.ActiveCollectableDatas)
            {
                AwaitingRootView arv = Object.Instantiate<AwaitingRootView>(_awaitingRootViewPrefab, _awaitingRoot);
                arv.Initialize(kvp.Key, kvp.Value.Count);
                _awaitingRootViews.Add(arv);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(_awaitingRoot);
        }

        private void ClearAwaitingRootViews()
        {
            foreach (var avs in _awaitingRootViews)
            {
                avs.Dispose();
            }

            _awaitingRootViews.Clear();
        }

        private const float CollectAnimationDuration = .2f;

        private void OnCollectableCollected(CollectableId collectableId, int itemIndex)
        {
            for (int i = _awaitingRootViews.Count - 1; i >= 0; i--)
            {
                AwaitingRootView avs = _awaitingRootViews[i];
                if (!InGameController.Instance.CollectableHandler.ActiveCollectableDatas.TryGetValue(avs.CollectableId, out List<CollectableData> collectableDatas))
                {
                    _awaitingRootViews.Remove(avs);
                    avs.Dispose();
                    Object.Destroy(avs.gameObject);
                    continue;
                }else
                {
                    avs.UpdateAmount(collectableDatas.Count);
                }
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(_awaitingRoot);

            int lastSameCollectableIndex = _collectableViews.FindLastIndex(t => t.CollectableId == collectableId);

            CollectableView collectableView = Object.Instantiate<CollectableView>(_collectableViewPrefab, transform);

            collectableView.Initialize(collectableId);

            collectableView.transform.position = _collectedRoots[++lastSameCollectableIndex].position;

            collectableView.transform.DOPunchScale(Vector3.zero, CollectAnimationDuration, vibrato: 1).SetLink(collectableView.gameObject);

            if (lastSameCollectableIndex == -1)
            {
                _collectableViews.Add(collectableView);
            }
            else
            {
                bool canClear = true;
                for (int i = 1; i <= 2; i++)
                {
                    if (lastSameCollectableIndex - i < 0 || _collectableViews[lastSameCollectableIndex - i].CollectableId != collectableId)
                    {
                        canClear = false; 
                        break;
                    }
                }

                if (canClear)
                {
                    DOVirtual.DelayedCall(CollectAnimationDuration, () =>
                    {
                        collectableView.transform.DOScale(Vector3.zero, CollectAnimationDuration).OnComplete(() => {
                            collectableView.Dispose();
                            Object.Destroy(collectableView.gameObject);
                            }).SetLink(collectableView.gameObject);
                    }).SetLink(gameObject);

                    for (int i = 1; i <= 2; i++)
                    {
                        CollectableView prevCollectableViews = _collectableViews[lastSameCollectableIndex - i];
                        _collectableViews.RemoveAt(lastSameCollectableIndex - i);
                        DOVirtual.DelayedCall(CollectAnimationDuration, () =>
                        {
                            prevCollectableViews.transform.DOScale(Vector3.zero, CollectAnimationDuration).OnComplete(() =>
                            {
                                prevCollectableViews.Dispose();
                                Object.Destroy(prevCollectableViews.gameObject);
                            }).SetLink(prevCollectableViews.gameObject);
                        }).SetLink(gameObject);
                    }

                }
                else
                {
                    _collectableViews.Insert(lastSameCollectableIndex, collectableView);

                    for (int i = 0; i < _collectableViews.Count; i++)
                    {
                        if (i == lastSameCollectableIndex) continue;
                        CollectableView collectableView1 = _collectableViews[i];
                        collectableView1.transform.DOKill();
                        collectableView1.transform.DOMove(_collectedRoots[i].position, CollectAnimationDuration).SetLink(collectableView1.gameObject);
                    }
                }
            }

            
        }
    }
}
