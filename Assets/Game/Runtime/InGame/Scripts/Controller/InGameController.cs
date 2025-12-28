using Game.Runtime.InGame.Models.Level;
using Game.Runtime.InGame.Scripts.Utility;
using Game.Runtime.InitializeHelper;
using Game.Runtime.PanelHandler;
using Game.Runtime.PlayerData;
using Game.Runtime.Scripts.UI.LevelFailed;
using Game.Runtime.Scripts.UI.LevelSuccess;
using Game.SingletonHelper;
using UnityEngine;

namespace Game.Runtime.InGame.Scripts.Controller
{
    public class InGameController : SingletonBehaviour<InGameController>, IInitializable
    {
        public bool MarkDisposeOnSceneChange => true;

        internal UpdateHandler GameUpdateHandler { get; private set; }
        internal GameDurationHandler GameDurationHandler { get; private set; }
        internal CameraHandler CameraHandler { get; private set; }
        internal CollectableHandler CollectableHandler { get; private set; }

        internal LevelData LevelData { get; private set; }

        internal int ActiveLevel { get; private set; }


        protected override void OnAwake()
        {
            InitializeController.Instance.RegisterInitialize(this);
        }

        public void Initialize()
        {
            ActiveLevel = PlayerDataController.Instance.AccountData.GameLevel;

            LevelData = LevelUtility.GetLevel(ActiveLevel);

            GameUpdateHandler = new UpdateHandler();

            GameUpdateHandler.Initialize();

            GameDurationHandler = new GameDurationHandler(LevelData.LevelDuration, OnTimeOver);

            GameDurationHandler.Initialize();
            GameDurationHandler.StartTick();

            CollectableHandler = new CollectableHandler(LevelData.CollectableData, LevelData.CollectablesRootData, OnAllCollected, OnSlotFullFail);

            CollectableHandler.Initialize();

            CameraHandler = new CameraHandler(Camera.main, LevelData.MapData, availableZoom: 10);
            CameraHandler.Initialize();

            GameUpdateHandler.Register(GameDurationHandler);
            GameUpdateHandler.Register(CollectableHandler);
            GameUpdateHandler.Register(CameraHandler);
        }

        public void Dispose()
        {
            GameDurationHandler.Dispose();
            CollectableHandler.Dispose();
            CameraHandler.Dispose();

            GameUpdateHandler.Unregister(GameDurationHandler);
            GameUpdateHandler.Unregister(CollectableHandler);
            GameUpdateHandler.Unregister(CameraHandler);

            GameUpdateHandler.Dispose();
        }

        private void OnTimeOver()
        {
            PanelController.Instance.OpenPopup(new LevelFailedPopupParams());
        }

        private void OnAllCollected()
        {
            PlayerDataController.Instance.AccountData.GameLevel += 1;
            PlayerDataController.Instance.SaveAccountData();
            PanelController.Instance.OpenPopup(new LevelSuccessPopupParams());
        }

        private void OnSlotFullFail()
        {
            GameDurationHandler.StopTick();
            PanelController.Instance.OpenPopup(new LevelFailedPopupParams());
        }
        
    }
}
