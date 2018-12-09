using Newtonsoft.Json;

namespace Eos.Models
{
    public class BuyRamData
    {
        [JsonProperty("payer")]
        public Name Payer { get; set; }

        [JsonProperty("receiver")]
        public Name Receiver { get; set; }

        [JsonProperty("bytes")]
        public int Bytes { get; set; }
    }

    public class BuyRamAction : Action<BuyRamData>
    {
        public override Name Account => "eosio";

        public override Name Name => "buyrambytes";
    }
}
