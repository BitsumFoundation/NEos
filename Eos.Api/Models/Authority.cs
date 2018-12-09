using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace Eos.Models
{
    public class Authority
    {
        [JsonProperty("threshold")]
        public uint Threshold { get; set; }

        [JsonProperty("keys")]
        public List<AuthorityKey> Keys { get; set; }

        //TODO: uncompleted properties
        [JsonProperty("accounts")]
        public IList Accounts { get; set; } = new ArrayList();

        [JsonProperty("waits")]
        public IList Waits { get; set; } = new ArrayList();
    }
}
