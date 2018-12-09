using Newtonsoft.Json;

namespace Eos.Models
{
    public class NewAccountData
    {
        [JsonProperty("creator")]
        public Name Creator { get; set; }

        [JsonProperty("name")]
        public Name Name { get; set; }

        [JsonProperty("owner")]
        public Authority Owner { get; set; }

        [JsonProperty("active")]
        public Authority Active { get; set; }
    }

    public class NewAccountAction : Action<NewAccountData>
    {
        public override Name Account => "eosio";

        public override Name Name => "newaccount";
    }
}
