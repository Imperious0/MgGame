using Newtonsoft.Json;
using UnityEngine;

namespace Game.Runtime.JsonUtils.JsonConverters
{
    public class Vector3Converter : JsonConverter<Vector3>
    {
        public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("x"); writer.WriteValue(value.x);
            writer.WritePropertyName("y"); writer.WriteValue(value.y);
            writer.WritePropertyName("z"); writer.WriteValue(value.z);
            writer.WriteEndObject();
        }

        public override Vector3 ReadJson(JsonReader reader, System.Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            float x = 0, y = 0, z = 0;

            if(reader.TokenType == JsonToken.StartObject)
            {
                reader.Read();

                while (reader.TokenType != JsonToken.EndObject)
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        string propertyName = reader.Value.ToString().ToLower();
                        reader.Read();
                        string value = reader.Value.ToString();

                        switch (propertyName)
                        {
                            case "x":
                                x = float.Parse(value);
                                break;
                            case "y":
                                y = float.Parse(value);
                                break;
                            case "z":
                                z = float.Parse(value);
                                break;
                        }
                    }
                    reader.Read();

                }
            }
            else
            {
                throw new MissingReferenceException($"Cant Deserialize object correctly {objectType}");
            }


                // Eğer hiçbir değer okunmadıysa bile uygulama çökmez, 0 döner
                return new Vector3(x, y, z);
        }
    }
}
