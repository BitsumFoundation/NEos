using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eos.Api.Chain
{
    internal class BalanceRequest
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }
    }
}
