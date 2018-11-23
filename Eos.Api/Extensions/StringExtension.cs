using Newtonsoft.Json;

namespace Eos
{
    public static class StringExtension
    {
        public static T FromJson<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
