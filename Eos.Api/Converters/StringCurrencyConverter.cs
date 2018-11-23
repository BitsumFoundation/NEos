using Newtonsoft.Json;
using System;

namespace Eos
{
    internal class StringCurrencyConverter : JsonConverter
    {
        public override bool CanRead => true;

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(string)) return true;

            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return new Currency(reader.Value.ToString());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}
