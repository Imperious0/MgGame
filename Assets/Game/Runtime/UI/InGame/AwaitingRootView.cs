using Game.Runtime.InGame.Models;
using TMPro;
using UnityEngine;

namespace Game.Runtime.UI.InGame
{
    internal class AwaitingRootView : MonoBehaviour
    {
        [SerializeField] private CollectableView _collectableView;
        [SerializeField] private TextMeshProUGUI _amountText;

        public CollectableId CollectableId { get => _collectableView.CollectableId; }
        public void Initialize(CollectableId collectableId, int amount)
        {
            _collectableView.Initialize(collectableId);
            UpdateAmount(amount);
        }

        public void Dispose()
        {
            _collectableView.Dispose();
        }

        public void UpdateAmount(int amount)
        {
            _amountText.text = amount.ToString();
        }
    }
}
