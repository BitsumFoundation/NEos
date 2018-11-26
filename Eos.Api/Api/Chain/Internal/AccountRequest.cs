using Newtonsoft.Json;

namespace Eos.Api.Chain
{
    internal class AccountRequest
    {
        [JsonProperty("account_name")]
        public string AccountName { get; set; }
    }
}
