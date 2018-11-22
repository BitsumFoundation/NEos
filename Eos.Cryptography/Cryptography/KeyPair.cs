namespace Eos.Cryptography
{
    public class KeyPair
    {
        public KeyPair(KeyTypes keyType)
        {
            KeyType = keyType;
            PrivateKey = new PrivateKey();
            PublicKey = new PublicKey(PrivateKey);
        }

        public KeyPair(string privateKey)
        {
            PrivateKey = new PrivateKey(privateKey);
            PublicKey = new PublicKey(PrivateKey);
            KeyType = PrivateKey.KeyType;
        }

        public KeyTypes KeyType { get; private set; }

        public PrivateKey PrivateKey { get; private set; }

        public PublicKey PublicKey { get; private set; }

        public override string ToString()
        {
            return PublicKey.ToString();
        }

        public string SignData(string chainId, string data)
        {
            return PrivateKey.SignData(chainId, data);
        }

        public bool VerifySignature(string chainId, string data, string signature)
        {
            return PublicKey.VerifySignature(chainId, data, signature);
        }
    }
}
