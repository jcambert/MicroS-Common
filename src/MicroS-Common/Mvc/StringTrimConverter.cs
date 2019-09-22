using Newtonsoft.Json;
using System;

namespace MicroS_Common.Mvc
{
    public class StringTrimConverter : JsonConverter
    {
        public StringTrimConverter():base()
        {

        }
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                if (reader.Value != null)
                {
                    return (reader.Value as string).Trim();
                }
            }
            return reader.Value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string str = (string)value;
            if (str == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteValue(str.Trim());
            }
        }
    }
}
