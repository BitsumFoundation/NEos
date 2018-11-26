using Newtonsoft.Json;
using System.Collections.Generic;

namespace Eos.Api.Chain
{
    internal class PushTransactionRequest
    {
        [JsonProperty("signatures")]
        public List<string> Signatures { get; set; }

        [JsonProperty("compression")]
        public string Compression { get; set; }

        [JsonProperty("packed_trx")]
        public string PackedTransaction { get; set; }
    }
}
