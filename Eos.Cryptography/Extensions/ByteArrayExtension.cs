using System;
using System.Linq;
using System.Numerics;

namespace Eos
{
    using Cryptography;

    public static class ByteArrayExtension
    {
        private static readonly byte[] ZeroBuf = new byte[] { 0 };

        public static string ToHex(this byte[] value)
        {
            return string.Concat(value.Select(b => $"{b:X2}"));
        }

        public static string ToBase58(this byte[] value)
        {
            return Base58.Encode(value);
        }

        public static BigInteger ToInt256(this byte[] value, int offset = 0)
        {
            byte[] buf = new byte[32];

            Array.Copy(value, offset, buf, 0, 32);

            BigInteger res = new BigInteger(buf.Reverse().Concat(ZeroBuf));

            return res;
        }

        public static byte[] Concat(this byte[] value, params byte[][] buffers)
        {
            byte[] res = new byte[value.Length + buffers.Sum(x => x != null ? x.Length : 0)];
            value.CopyTo(res, 0);
            int offset = value.Length;
            foreach (var item in buffers)
            {
                if (item == null) continue;

                Array.Copy(item, 0, res, offset, item.Length);
                offset += item.Length;
            }

            return res;
        }

        public static byte[] Reverse(this byte[] value)
        {
            byte[] res = new byte[value.Length];

            for (int i = 0; i < res.Length; i++)
            {
                res[i] = value[value.Length - i - 1];
            }

            return res;
        }

        public static byte[] TakePart(this byte[] value, int offset, int length)
        {
            byte[] res = new byte[length];

            Array.Copy(value, offset, res, 0, length);

            return res;
        }
    }
}
