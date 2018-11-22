using System;
using System.Numerics;
using System.Security.Cryptography;

namespace Eos.Cryptography
{
    public class PublicKey : KeyBase
    {
        public PublicKey(PrivateKey privateKey)
        {
            KeyType = privateKey.KeyType;

            Q = privateKey.Curve.G * privateKey.D;
            Value = Q.Encode();

            switch (KeyType)
            {
                case KeyTypes.K1:
                    {                       
                        ToK1Key(false);
                        break;
                    }
                case KeyTypes.R1:
                    {
                        ToR1Key(false);
                        break;
                    }
                default: throw new InvalidKeyException(Resources.WrongKeyType);
            }
        }

        public PublicKey(string publicKey)
        {
            StringValue = publicKey;

            if (StringValue.StartsWith(R1PPublicPrefix))
            {
                KeyType = KeyTypes.R1;
                FromR1Key();
            }
            else
            {
                KeyType = KeyTypes.K1;
                FromK1Key(false);
            }

            Q = Point.FromX(Value.ToInt256(1), Value[0] == 3, Curve);
        }

        public override int Length => 33;

        public Point Q { get; set; }

        public bool VerifySignature(byte[] data, string signature)
        {
            if (!signature.StartsWith(Signature.K1Prefix) && !signature.StartsWith(Signature.R1Prefix))
            {
                return false;
            }

            byte[] buf = Base58.Decode(signature.Substring(Signature.PrefixLength));

            if (buf.Length != Signature.Length + ChecksumLength)
            {
                return false;
            }

            byte[] signatureBuf = new byte[buf.Length - ChecksumLength];
            Array.Copy(buf, 0, signatureBuf, 0, Signature.Length);

            byte[] checksum;

            if (signature.StartsWith(Signature.R1Prefix))
            {
                checksum = RIPEMD160.ComputeHash(signatureBuf.Concat(R1Salt)).TakePart(0, 4);
            }
            else
            {
                checksum = RIPEMD160.ComputeHash(signatureBuf.Concat(K1Salt)).TakePart(0, 4);
            }

            if (!CheckChecksum(buf, checksum))
            {
                return false;
            }

            byte[] hash;

            using (SHA256 sha = SHA256.Create())
            {
                hash = sha.ComputeHash(data, 0, data.Length);
            }

            byte recoveryId = (byte)(signatureBuf[0] - 31);
            BigInteger r = signatureBuf.ToInt256(1);
            BigInteger s = signatureBuf.ToInt256(33);

            var q = RecoverPublicKey(hash, r, s, recoveryId, Curve);

            if (q.X.Value != Q.X.Value && q.Y.Value != Q.Y.Value)
            {
                return false;
            }

            if (r.Sign < 1 || s.Sign < 1 || r.CompareTo(Curve.N) >= 0 || s.CompareTo(Curve.N) >= 0) return false;

            BigInteger e = hash.ToInt256();
            BigInteger c = s.ModInverse(Curve.N);
            BigInteger u1 = (e * c).Mod(Curve.N);
            BigInteger u2 = (r * c).Mod(Curve.N);
            Point R = Curve.G.MultiplyTwo(u1, q, u2);
            BigInteger v = R.X.Value.Mod(Curve.N);

            return v.Equals(r);
        }

        public bool VerifySignature(string chainId, string data, string signature)
        {
            byte[] chainIdBuf = chainId.FromHex();
            byte[] dataBuf = data.FromHex();

            byte[] buf = chainIdBuf.Concat(dataBuf, new byte[32]);

            return VerifySignature(buf, signature);
        }
    }
}
