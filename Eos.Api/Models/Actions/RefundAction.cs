using Newtonsoft.Json;

namespace Eos.Models
{
    public class RefundData
    {
        [JsonProperty("owner")]
        public Name Owner { get; set; }
    }

    public class RefundAction : Action<RefundData>
    {
        public override Name Account => "eosio";

        public override Name Name => "refund";
    }
}
