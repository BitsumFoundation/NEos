using Newtonsoft.Json;

namespace Eos.Models
{
    public class StakeData
    {
        [JsonProperty("from")]
        public Name From { get; set; }

        [JsonProperty("receiver")]
        public Name Receiver { get; set; }

        [JsonProperty("stake_net_quantity")]
        public Currency Net { get; set; }

        [JsonProperty("stake_cpu_quantity")]
        public Currency Cpu { get; set; }

        [JsonProperty("transfer")]
        public bool Transfer { get; set; }
    }

    public class StakeAction : Action<StakeData>
    {
        public override Name Account => "eosio";

        public override Name Name => "delegatebw";
    }
}
