using System.Collections.Generic;
using System.IO;

namespace Eos.Models
{
    public class GenericAction : IAction
    {
        public Name Account { get; set; }

        public virtual Name Name { get; set; }

        public List<Authorization> Authorization { get; set; }

        public virtual object RawData { get; set; }

        public string HexData { get; set; }

        public void WriteBytes(BinaryWriter writer)
        {
            Account.WriteBytes(writer);
            Name.WriteBytes(writer);
            writer.WriteVarUInt32((uint)Authorization.Count);
            foreach (var item in Authorization)
            {
                item.WriteBytes(writer);
            }

            byte[] buf = HexData.FromHex();
            writer.WriteVarUInt32((uint)buf.Length);
            writer.Write(buf);
        }
    }
}
