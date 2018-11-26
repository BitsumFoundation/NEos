using Newtonsoft.Json;
using System.IO;

namespace Eos.Models
{
    public class Authorization : IBinaryWritable
    {
        [JsonProperty("actor")]
        public Name Actor { get; set; }

        [JsonProperty("permission")]
        public Name Permission { get; set; }

        public void WriteBytes(BinaryWriter writer)
        {
            Actor.WriteBytes(writer);
            Permission.WriteBytes(writer);
        }
    }
}
