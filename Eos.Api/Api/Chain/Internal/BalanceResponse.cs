using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eos.Api.Chain
{
    internal class BalanceResponse
    {
        [JsonProperty("assets")]
        public List<string> Assets { get; set; }
    }
}
