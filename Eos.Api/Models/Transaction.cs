using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Eos.Models
{
    public class Transaction : IBinaryWritable
    {
        [JsonProperty("expiration")]
        [JsonConverter(typeof(EosDateTimeConverter))]
        public DateTime Expiration { get; set; }

        [JsonProperty("ref_block_num")]
        public uint RefBlockNum { get; set; }

        [JsonProperty("ref_block_prefix")]
        public uint RefBlockPrefix { get; set; }

        [JsonProperty("max_net_usage_words")]
        public uint MaxNetUsageWords { get; set; }

        [JsonProperty("max_cpu_usage_ms")]
        public byte MaxCpuUsageMs { get; set; }

        [JsonProperty("delay_sec")]
        public uint DelaySec { get; set; }

        [JsonProperty("actions")]
        public List<IAction> Actions { get; set; }

        [JsonIgnore]
        internal string ChainId { get; set; }

        public void WriteBytes(BinaryWriter writer)
        {
            writer.Write(Expiration.ToUnixTime());
            writer.Write((short)(RefBlockNum & 0xFFFF));
            writer.Write((uint)(RefBlockPrefix & 0xFFFFFFFF));
            writer.WriteVarUInt32(MaxNetUsageWords);
            writer.WriteVarUInt32(MaxCpuUsageMs);
            writer.WriteVarUInt32(DelaySec);
            writer.WriteVarUInt32(0);
            writer.WriteVarUInt32((uint)Actions.Count);
            foreach (var item in Actions)
            {
                item.WriteBytes(writer);
            }
            writer.WriteVarUInt32(0);
        }
    }
}
