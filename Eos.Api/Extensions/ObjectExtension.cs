using Newtonsoft.Json;

namespace Eos
{
    public static class ObjectExtension
    {
        public static string ToJson(this object value)
        {
            return JsonConvert.SerializeObject(value, Formatting.Indented);
        }
    }
}
