using Newtonsoft.Json;
using System.Collections.Generic;

namespace Eos.Api.History
{
    internal class KeyAccountResponse
    {
        [JsonProperty("account_names")]
        public List<string> AccountNames { get; set; }
    }
}
