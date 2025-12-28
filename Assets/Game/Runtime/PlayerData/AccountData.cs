using Newtonsoft.Json;

namespace Game.Runtime.PlayerData
{
    public class AccountData
    {
        public int GameLevel;

        public bool SoundState;
        public bool MusicState;
        public bool VibrationState;

        [JsonConstructor]
        public AccountData(int gameLevel, bool soundState, bool musicState, bool vibrationState)
        {
            GameLevel = gameLevel;
            SoundState = soundState;
            MusicState = musicState;
            VibrationState = vibrationState;
        }

        public static AccountData CreateDefault() => new AccountData(gameLevel: 1, soundState: true, musicState: true, vibrationState: true);
    }
}
