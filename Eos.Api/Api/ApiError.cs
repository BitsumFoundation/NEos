using Newtonsoft.Json;

namespace Eos.Api
{
    public class ApiError
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("error")]
        public ApiErrorEx Error { get; set; }
    }

    public class ApiErrorEx
    {
    }
}
