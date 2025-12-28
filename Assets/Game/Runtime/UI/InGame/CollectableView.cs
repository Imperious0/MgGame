using Game.Runtime.Bootstrapper;
using Game.Runtime.InGame.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Runtime.UI.InGame
{
    internal class CollectableView : MonoBehaviour
    {
        [SerializeField] private Image _collectableImage;
        public CollectableId CollectableId { get; private set; }

        public void Initialize(CollectableId collectableId)
        {
            CollectableId = collectableId;
            _collectableImage.sprite = GameController.Instance.PrefabDatabase.GetCollectableSprite(collectableId);
        }

        public void Dispose()
        {

        }
    }
}
