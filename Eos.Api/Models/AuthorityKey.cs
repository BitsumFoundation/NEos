using Newtonsoft.Json;

namespace Eos.Models
{
    using Cryptography;

    public class AuthorityKey
    {
        [JsonProperty("key")]
        [JsonConverter(typeof(StringPublicKeyConverter))]
        public PublicKey Key { get; set; }

        [JsonProperty("weight")]
        public ushort Weight { get; set; }
    }
}
