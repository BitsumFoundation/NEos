using Eos.Models;
using Newtonsoft.Json;

namespace Eos.Api.Chain
{
    internal class AbiJsonToBinRequest
    {
        [JsonProperty("code")]
        public Name Code { get; set; }

        [JsonProperty("action")]
        public Name Action { get; set; }

        [JsonProperty("args")]
        public object Args { get; set; }
    }
}
