namespace NEos.Cryptography
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
        }

        public override int Length => 33;

        public Point Q { get; set; }

    }
}
