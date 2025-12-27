using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Game.Runtime.JsonUtils.JsonConverters
{
    public class EnumConverter<TEnum> : JsonConverter<TEnum> where TEnum : struct, Enum  
    {
        public override void WriteJson(JsonWriter writer, TEnum value, JsonSerializer serializer)
        {
            writer.WriteValue(value.Equals(default(TEnum)) ? String.Empty : value.ToString());
        }

        public override TEnum ReadJson(JsonReader reader, Type objectType, TEnum existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            try
            {
                var value = reader.Value.ToString();

                if (string.IsNullOrWhiteSpace(value))
                {
                    return default;
                }

                return Enum.Parse<TEnum>(value);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Cant deserialize Enum by EnumConverter<T> : {reader.Value}");
                Debug.LogWarning(e.Message);
                Debug.LogWarning(e.StackTrace);
            }

            return default;
        }
    }
}
