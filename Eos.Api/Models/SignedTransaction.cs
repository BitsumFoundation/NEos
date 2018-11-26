using Newtonsoft.Json;
using System.Collections.Generic;

namespace Eos.Models
{
    public class SignedTransaction
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("signatures")]
        public List<string> Signatures { get; set; }

        [JsonProperty("compression")]
        public string Compression { get; set; }

        [JsonProperty("packed_trx")]
        public string PackedTransaction { get; set; }

        [JsonProperty("transaction")]
        public Transaction Transaction { get; set; }
    }
}
