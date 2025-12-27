using Game.Runtime.JsonUtils.JsonConverters;
using Newtonsoft.Json;

namespace Game.Runtime.InGame.Models
{
    [JsonConverter(typeof(EnumConverter<CollectableId>))]
    public enum CollectableId : ushort
    {
        Invalid = 0,

        Tower_1 = 1,
        Tower_2 = 2,
    }
}
