using Newtonsoft.Json;

namespace Game.Runtime.PlayerData
{
    public class AccountData
    {
        public int GameLevel;

        [JsonConstructor]
        public AccountData(int gameLevel)
        {
            GameLevel = gameLevel;
        }

        public static AccountData CreateDefault() => new AccountData(gameLevel: 1);
    }
}
