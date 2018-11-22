using System;

namespace NEos
{
    using Cryptography;

    public static class StringExtension
    {
        public static byte[] FromHex(this string value)
        {
            byte[] result = new byte[value.Length / 2];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (byte)Convert.ToInt32(value.Substring(i * 2, 2), 16);
            }

            return result;
        }

        public static byte[] FromBase58(this string value)
        {
            return Base58.Decode(value);
        }
    }
}
