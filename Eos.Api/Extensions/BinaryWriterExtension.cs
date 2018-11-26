using System.IO;

namespace Eos
{
    public static class BinaryWriterExtension
    {
        public static void WriteVarUInt32(this BinaryWriter writer, uint value)
        {
            while (true)
            {
                var val = (byte)(value & 0x7f);
                var remaining = (byte)(value >> 7);

                if (remaining > 0)
                {
                    writer.Write((byte)(0x80 | val));
                }
                else
                {
                    writer.Write(val);
                    break;
                }
            }
        }
    }
}
