using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eos.Models
{
    public class Block
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("block_num")]
        public uint BlockNum { get; set; }

        [JsonProperty("ref_block_prefix")]
        public uint RefBlockPrefix { get; set; }
    }
}
