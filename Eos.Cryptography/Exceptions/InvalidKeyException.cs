using System;

namespace Eos.Cryptography
{
    public class InvalidKeyException : Exception
    {
        public InvalidKeyException(string message) : base(message) { }
    }
}
