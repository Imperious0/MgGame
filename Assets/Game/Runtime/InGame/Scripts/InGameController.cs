using Game.Runtime.InGame.Models.Level;
using Game.Runtime.InGame.Scripts;
using Game.Runtime.InitializeHelper;
using Game.Runtime.PlayerData;
using Game.SingletonHelper;

namespace Game.Runtime.Scripts
{
    public class InGameController : SingletonBehaviour<InGameController>, IInitializable
    {
        internal UpdateHandler GameUpdateHandler { get; private set; }
        internal GameDurationHalder GameDurationHandler { get; private set; }
        internal LevelData LevelData { get; private set; }

        protected override void OnAwake()
        {
            InitializeController.Instance.RegisterInitialize(this);
        }

        public void Initialize()
        {
            int levelIndex = PlayerDataController.Instance.AccountData.GameLevel;

            LevelData = LevelUtility.GetLevel(levelIndex);

            GameUpdateHandler = new UpdateHandler();

            GameUpdateHandler.Initialize();

            GameDurationHandler = new GameDurationHalder(LevelData.LevelDuration, OnTimeOver);

            GameDurationHandler.Initialize();


        }

        public void OnTimeOver()
        {
            Dispose();
        }

        public void Dispose()
        {
            GameDurationHandler.Dispose();

            GameUpdateHandler.Dispose();
        }
    }
}
