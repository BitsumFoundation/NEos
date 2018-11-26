using Newtonsoft.Json;
using System.IO;

namespace Eos.Models
{
    using Cryptography;

    [JsonConverter(typeof(StringNameConverter))]
    public class Name : IBinaryWritable
    {
        private string value;

        public Name(string name)
        {
            value = name;
        }

        public override string ToString()
        {
            return value;
        }

        public static implicit operator string(Name value)
        {
            return value.value;
        }

        public static implicit operator Name(string value)
        {
            return new Name(value);
        }

        public void WriteBytes(BinaryWriter writer)
        {
            writer.Write(Base32.Encode(value));
        }
    }
}
