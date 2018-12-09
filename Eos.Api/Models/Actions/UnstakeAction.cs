using Newtonsoft.Json;

namespace Eos.Models
{
    public class UnstakeData
    {
        [JsonProperty("from")]
        public Name From { get; set; }

        [JsonProperty("receiver")]
        public Name Receiver { get; set; }

        [JsonProperty("unstake_net_quantity")]
        public Currency Net { get; set; }

        [JsonProperty("unstake_cpu_quantity")]
        public Currency Cpu { get; set; }

        [JsonProperty("transfer")]
        public bool Transfer { get; set; }
    }

    public class UnstakeAction : Action<UnstakeData>
    {
        public override Name Account => "eosio";

        public override Name Name => "undelegatebw";
    }
}
