using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace NEos.Cryptography
{
    public abstract class KeyBase
    {
        public const string K1PublicPrefix = "EOS";
        public const string R1PPublicPrefix = "PUB_R1_";
        public const string R1PrivatePrefix = "PVT_R1_";

        protected const int ChecksumLength = 4;
        protected static readonly byte[] K1Salt = Encoding.UTF8.GetBytes("K1");
        protected static readonly byte[] R1Salt = Encoding.UTF8.GetBytes("R1");

        public abstract int Length { get; }

        public KeyTypes KeyType { get; protected set; }

        public EllipticCurve Curve => KeyType == KeyTypes.R1 ? EllipticCurve.SECP256R1 : EllipticCurve.SECP256K1;

        public byte[] Value { get; protected set; }

        protected string StringValue { get; set; }

        public override string ToString()
        {
            return StringValue;
        }

        public static byte[] ComputeDoubleSHA256Checksum(byte[] buffer, int offset, int length)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] hash1 = sha.ComputeHash(buffer, offset, length);
                byte[] hash2 = sha.ComputeHash(hash1);
                byte[] res = new byte[ChecksumLength];
                Array.Copy(hash2, 0, res, 0, ChecksumLength);

                return res;
            }
        }

        public static byte[] ComputeK1Checksum(byte[] buffer, int offset, int length)
        {
            byte[] hash = RIPEMD160.ComputeHash(buffer, offset, length);
            byte[] result = new byte[ChecksumLength];
            Array.Copy(hash, 0, result, 0, ChecksumLength);

            return result;
        }

        public static byte[] ComputeR1Checksum(byte[] buffer, int offset, int length)
        {
            byte[] buf = new byte[length + R1Salt.Length];
            Array.Copy(buffer, offset, buf, 0, length);
            Array.Copy(R1Salt, 0, buf, buf.Length - R1Salt.Length, R1Salt.Length);

            byte[] hash = RIPEMD160.ComputeHash(buf);
            byte[] result = new byte[ChecksumLength];
            Array.Copy(hash, result, ChecksumLength);

            return result;
        }
        
        protected void ToK1Key(bool isPrivate)
        {
            byte[] checksum;
            string prefix = string.Empty;
            byte[] buf;
            if (isPrivate)
            {
                buf = new byte[Value.Length + ChecksumLength + 1];
                buf[0] = 0x80;
                Array.Copy(Value, 0, buf, 1, Value.Length);
                checksum = ComputeDoubleSHA256Checksum(buf, 0, buf.Length - ChecksumLength);
            }
            else
            {
                buf = new byte[Value.Length + ChecksumLength];
                Array.Copy(Value, 0, buf, 0, Value.Length);
                checksum = ComputeK1Checksum(Value, 0, Value.Length);
                prefix = K1PublicPrefix;
            }

            Array.Copy(checksum, 0, buf, buf.Length - ChecksumLength, ChecksumLength);
            StringValue = prefix + buf.ToBase58();
        }

        protected void FromK1Key(bool isPrivate)
        {
            var buf = (isPrivate ? StringValue : StringValue.Replace(K1PublicPrefix, string.Empty)).FromBase58();

            if (buf.Length != 37)
            {
                throw new InvalidKeyException(Resources.WrongKeyLength);
            }

            Value = new byte[Length];
            if (isPrivate)
            {
                if (buf[0] != 0x80) // Bitcoin MainNetCode
                {
                    throw new InvalidKeyException(Resources.WrongKeyPrefix);
                }

                Array.Copy(buf, 1, Value, 0, Length);
            }
            else
            {
                Array.Copy(buf, 0, Value, 0, Length);
            }       
        }

        protected void ToR1Key(bool isPrivate)
        {
            byte[] buf = new byte[Value.Length + ChecksumLength];

            Array.Copy(Value, buf, Value.Length);

            byte[] checksum = ComputeR1Checksum(buf, 0, Value.Length);

            Array.Copy(checksum, 0, buf, buf.Length - ChecksumLength, ChecksumLength);

            StringValue = buf.ToBase58();
            StringValue = (isPrivate ? R1PrivatePrefix : R1PPublicPrefix) + StringValue;
        }

        protected void FromR1Key()
        {
            bool isPrivate = StringValue.StartsWith(R1PrivatePrefix);
            bool isPublic = StringValue.StartsWith(R1PPublicPrefix);

            if (!isPrivate && !isPublic)
            {
                throw new InvalidKeyException(Resources.WrongKeyPrefix);
            }

            byte[] buf, checksum;
            if (isPrivate)
            {
                buf = StringValue.Replace(R1PrivatePrefix, string.Empty).FromBase58();
            }
            else
            {
                buf = StringValue.Replace(R1PPublicPrefix, string.Empty).FromBase58();
            }

            checksum = ComputeR1Checksum(buf, 0, buf.Length - ChecksumLength);

            int length = buf.Length - ChecksumLength;

            if (length != Length)
            {
                throw new InvalidKeyException(Resources.WrongKeyLength);
            }

            if (!CheckChecksum(buf, checksum))
            {
                throw new InvalidKeyException(Resources.WrongKeyChecksum);
            }

            Value = new byte[length];
            Array.Copy(buf, Value, length);
        }

        protected bool CheckChecksum(byte[] buffer, byte[] checksum)
        {
            int j = 0;
            for (int i = buffer.Length - ChecksumLength; i < buffer.Length; i++)
            {
                if (buffer[i] != checksum[j]) return false;
                else j++;
            }

            return true;
        }

        protected static Point RecoverPublicKey(byte[] hash, BigInteger r, BigInteger s, int recoveryId, EllipticCurve curve)
        {
            var isYOdd = recoveryId & 1;
            var isSecondKey = recoveryId >> 1;
            var x = isSecondKey != 0 ? r + curve.N : r;

            var R = Point.FromX(x, isYOdd != 0, EllipticCurve.SECP256K1);
            var nR = R * curve.N;

            var eNeg = BigInteger.Negate(hash.ToInt256()).Mod(curve.N);
            var rInv = r.ModInverse(curve.N);

            var q = R.MultiplyTwo(s, curve.G, eNeg) * rInv;

            return q;
        }
    }
}
