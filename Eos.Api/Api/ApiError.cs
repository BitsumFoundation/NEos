using Newtonsoft.Json;

namespace Eos.Api
{
    public class ApiError
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("what")]
        public string What { get; set; }
    }
}
