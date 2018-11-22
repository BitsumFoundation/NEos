using System;
using System.Diagnostics;
using System.Numerics;

namespace NEos.Cryptography
{
    using System.Security.Cryptography;

    public class Signature
    {
        public const int Length = 65;
        public const string K1Prefix = "SIG_K1_";
        public const string R1Prefix = "SIG_R1_";
        public const int PrefixLength = 7;
        
        public Signature(string signature)
        {

        }

        public Signature(BigInteger r, BigInteger s, EllipticCurve curve)
        {
            R = r;
            S = s;
            Curve = curve;
        }

        public BigInteger R { get; private set; }

        public BigInteger S { get; private set; }

        public int RecoveryId { get; private set; }

        public EllipticCurve Curve { get; private set; }

        public byte[] Value { get; private set; }

        //protected string StringValue { get; set; }

        //public override string ToString()
        //{
        //    return StringValue;
        //}

        //public byte[] GetBytes()
        //{
        //    byte[] res = new byte[Length];

        //    byte[] buf = R.ToByteArray().Reverse();
        //    Array.Copy(buf, 0, res, 1, 32);

        //    buf = S.ToByteArray().Reverse();
        //    Array.Copy(buf, 0, res, 33, 32);

        //    return res;
        //}

    }
}
