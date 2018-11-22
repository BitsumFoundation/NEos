using System;
using System.Numerics;
using System.Security.Cryptography;

namespace NEos.Cryptography
{
    public partial class PrivateKey : KeyBase
    {
        public PrivateKey(KeyTypes keyType = KeyTypes.K1)
        {
            if (keyType != KeyTypes.K1 && keyType != KeyTypes.R1)
            {
                throw new InvalidKeyException(Resources.WrongKeyType);
            }

            KeyType = keyType;

            using (RandomNumberGenerator r = RandomNumberGenerator.Create())
            {
                Value = new byte[Length];
                r.GetBytes(Value);
            }

            D = Value.ToInt256();

            if (keyType == KeyTypes.R1) ToR1Key(true);
            else ToK1Key(true);
        }

        public PrivateKey(string privateKey)
        {
            StringValue = privateKey;

            if (StringValue.StartsWith(R1PrivatePrefix))
            {
                KeyType = KeyTypes.R1;
                FromR1Key();
                D = Value.ToInt256();
            }
            else
            {
                KeyType = KeyTypes.K1;
                FromK1Key(true);
                D = Value.ToInt256();
            }
        }

        public override int Length => 32;

        public BigInteger D { get; private set; }
        
        public string SignData(byte[] data)
        {
            byte[] hash;

            using (SHA256 sha = SHA256.Create())
            {
                hash = sha.ComputeHash(data, 0, data.Length);
            }

            Signature signature = SignHash(hash);

            byte[] checksum;
            string prefix;

            data = signature.GetBytes();

            switch (KeyType)
            {
                case KeyTypes.K1:
                    checksum = RIPEMD160.ComputeHash(data.Concat(K1Salt)).TakePart(0, 4);
                    prefix = Signature.K1Prefix;
                    break;
                case KeyTypes.R1:
                    checksum = RIPEMD160.ComputeHash(data.Concat(R1Salt)).TakePart(0, 4);
                    prefix = Signature.R1Prefix;
                    break;
                default: throw new InvalidKeyException(Resources.WrongKeyType);
            }

            string result = prefix + data.Concat(checksum).ToBase58();

            return result;
        }

        public string SignData(string chainId, string data)
        {
            byte[] chainIdBuf = chainId.FromHex();
            byte[] dataBuf = data.FromHex();

            byte[] buf = chainIdBuf.Concat(dataBuf, new byte[32]);

            return SignData(buf);
        }

        public Signature SignHash(byte[] hash)
        {
            if (hash.Length != 32)
            {
                throw new ArgumentException(nameof(hash));
            }

            ulong nonce = 0;

            Signature signature;

            do
            {
                signature = SignHash(hash, nonce++);

                if (signature.R.GetBitCount() < 256 && signature.S.GetBitCount() < 256)
                {
                    signature.RecoveryByte = (byte)(GetRecoveryByte(hash, signature.R, signature.S) + 31);

                    break;
                }

            } while (true);

            return signature;
        }

        private Signature SignHash(byte[] hash, ulong nonce)
        {
            BigInteger k, r, s;
            BigInteger e = hash.ToInt256();

            DeterministicGenerator kgen = new DeterministicGenerator(Curve.N, hash, Value, nonce);

            do
            {
                k = kgen.NextK();

                Point q = Curve.G * k;
                if (q.IsInfinity) continue;

                BigInteger x = q.X.Value;
                r = x.Mod(Curve.N);
                if (r.Sign == 0) continue;

                s = (k.ModInverse(Curve.N) * (e + D * r)).Mod(Curve.N);

                int i = r.GetBitCount();
                i = s.GetBitCount();

                break;
            }
            while (true);

            var overTwo = Curve.N >> 1;
            if (s.CompareTo(overTwo) > 0)
            {
                s = Curve.N - s;
            }

            Signature res = new Signature()
            {
                R = r,
                S = s
            };

            return res;
        }

        private byte GetRecoveryByte(byte[] hash, BigInteger r, BigInteger s)
        {
            var q = Curve.G * D;

            for (byte i = 0; i < 4; i++)
            {
                try
                {
                    var qq = RecoverPublicKey(hash, r, s, i, Curve);

                    if (qq.X.Value == q.X.Value && qq.Y.Value == q.Y.Value)
                    {
                        return i;
                    }
                }
                catch (Exception)
                {
                }
            }

            return 0;
        }
    }
}
