using Newtonsoft.Json;

namespace Eos.Models
{
    public class TransferData 
    {
        [JsonProperty("from")]
        public Name From { get; set; }

        [JsonProperty("to")]
        public Name To { get; set; }

        [JsonProperty("quantity")]
        public Currency Quantity { get; set; }

        [JsonProperty("memo")]
        public string Memo { get; set; }
    }

    public class TransferAction : Action<TransferData>
    {
        public override Name Account => "eosio.token";
        
        public override Name Name => "transfer";
    }
}
