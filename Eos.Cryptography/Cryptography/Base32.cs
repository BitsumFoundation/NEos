namespace Eos.Cryptography
{
    public static class Base32
    {
        public static byte CharToByte(char c)
        {
            if (c >= 'a' && c <= 'z') return (byte)(c - 'a' + 6);

            if (c >= '1' && c <= '5') return (byte)(c - '1' + 1);

            return 0;
        }

        public static ulong Encode(string data)
        {
            ulong result = 0;

            for (var i = 0; i <= 12; i++)
            {
                ulong c = 0;

                if (i < data.Length && i <= 12)
                {
                    c = CharToByte(data[i]);
                }

                if (i < 12)
                {
                    c &= 0x1f;
                    c <<= 64 - 5 * (i + 1);
                }
                else
                {
                    c &= 0x0f;
                }

                result |= c;
            }

            return result;
        }

        public static string Decode(ulong value)
        {
            const string charmap = ".12345abcdefghijklmnopqrstuvwxyz";

            var str = new char[13];
            ulong tmp = value;
            var count = -1;
            for (var i = 0; i <= 12; ++i)
            {
                var id = i == 0 ? tmp & 0x0f : tmp & 0x1f;
                char c = charmap[(byte)id];
                str[12 - i] = c;
                tmp >>= (i == 0 ? 4 : 5);

                if (id == 0 && count + 1 == i)
                    count = i;
            }

            return new string(str, 0, 12 - count);
        }
    }
}
