using Newtonsoft.Json;

namespace Eos.Api.History
{
    internal class KeyAccountRequest
    {
        [JsonProperty("public_key")]
        public string PublicKey { get; set; }
    }
}
