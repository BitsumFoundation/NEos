using System.IO;

namespace Eos.Models
{
    public interface IBinaryWritable
    {
        void WriteBytes(BinaryWriter writer);
    }
}
