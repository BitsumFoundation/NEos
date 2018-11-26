using Newtonsoft.Json;
using System.Collections.Generic;

namespace Eos.Models
{
    public interface IAction : IBinaryWritable
    {
        [JsonProperty("account")]
        Name Account { get; }

        [JsonProperty("name")]
        Name Name { get; }

        [JsonProperty("authorization")]
        List<Authorization> Authorization { get; }

        [JsonProperty("data")]
        object RawData { get; }

        [JsonProperty("hex_data")]
        string HexData { get; set; }
    }

    public interface IAction<T> : IAction
    {
        [JsonProperty("data")]
        T Data { get; }
    }
}
