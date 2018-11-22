using System.Numerics;
using System.Security.Cryptography;

namespace Eos.Cryptography
{
    public class DeterministicGenerator
    {
        private readonly byte[] zeroBuf = new byte[] { 0 };

        private BigInteger n;
        private byte[] k, v;
        private bool first;

        public DeterministicGenerator(BigInteger n, byte[] hash, byte[] key, ulong nonce)
        {
            if (nonce > 0)
            {
                hash = hash.Concat(new byte[nonce]);
            }

            this.n = n;
            k = new byte[32];
            v = new byte[32];
            for (int i = 0; i < v.Length; i++) v[i] = 1;

            using (HMACSHA256 hmac = new HMACSHA256(k))
            {
                k = hmac.ComputeHash(v.Concat(new byte[] { 0 }, key, hash));
                hmac.Key = k;
                v = hmac.ComputeHash(v);
                k = hmac.ComputeHash(v.Concat(new byte[] { 1 }, key, hash));
                hmac.Key = k;
                v = hmac.ComputeHash(v);
                v = hmac.ComputeHash(v);
            }

            first = true;
        }
        
        public BigInteger NextK()
        {
            BigInteger res = v.ToInt256();

            bool done = first;
            first = false;

            while (res.Sign <= 0 || res.CompareTo(n) >= 0 || !done)
            {
                using (HMACSHA256 hmac = new HMACSHA256(k))
                {
                    k = hmac.ComputeHash(v.Concat(zeroBuf));
                    hmac.Key = k;
                    v = hmac.ComputeHash(v);
                    v = hmac.ComputeHash(v);
                }

                res = v.ToInt256();

                done = true;
            }

            return res;
        }  
    }
}
