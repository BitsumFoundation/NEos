using System;
using System.Numerics;

namespace Eos.Cryptography
{
    public struct Signature
    {
        public const int Length = 65;
        public const string K1Prefix = "SIG_K1_";
        public const string R1Prefix = "SIG_R1_";
        public const int PrefixLength = 7;
        
        public BigInteger R { get; set; }

        public BigInteger S { get; set; }

        public byte RecoveryByte { get; set; }

        public byte[] GetBytes()
        {
            byte[] res = new byte[Length];

            byte[] buf = R.GetBytes();
            Array.Copy(buf, 0, res, 1, 32);

            buf = S.GetBytes();
            Array.Copy(buf, 0, res, 33, 32);

            res[0] = RecoveryByte;

            return res;
        }
    }
}
