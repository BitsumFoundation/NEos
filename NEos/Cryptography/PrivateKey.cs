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
        
    }
}
