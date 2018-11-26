using Newtonsoft.Json;

namespace Eos.Api.Chain
{
    using Models;

    internal class CurrencyStatsRequest
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }
    }

    public class CurrencyStats
    {
        [JsonProperty("supply")]
        [JsonConverter(typeof(StringCurrencyConverter))]
        public Currency Supply { get; set; }

        [JsonProperty("max_supply")]
        [JsonConverter(typeof(StringCurrencyConverter))]
        public Currency MaxSupply { get; set; }

        [JsonProperty("issuer")]
        public string Issuer { get; set; }

        [JsonIgnore]
        public string Ticker { get; set; }
    }
}
