using Newtonsoft.Json;

namespace Eos.Api.Chain
{
    internal class PushTransactionResponse
    {
        [JsonProperty("transaction_id")]
        public string TransactionId { get; set; }

        [JsonProperty("processed")]
        public object Processed { get; set; }
    }
}
