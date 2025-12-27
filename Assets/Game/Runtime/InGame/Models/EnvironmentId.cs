using Game.Runtime.JsonUtils.JsonConverters;
using Newtonsoft.Json;

namespace Game.Runtime.InGame.Models
{
    [JsonConverter(typeof(EnumConverter<EnvironmentId>))]
    public enum EnvironmentId
    {
        Invalid = 0,

        Tree_1 = 1,
    }
}
