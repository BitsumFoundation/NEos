using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Eos
{
    public class EosDateTimeConverter : DateTimeConverterBase
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var date = (DateTime)value;
            var iso = date.ToUniversalTime().ToString("s");

            writer.WriteValue(iso);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return DateTime.Parse(reader.Value.ToString());
        }
    }
}
