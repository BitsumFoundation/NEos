using System;

namespace NEos.Cryptography
{
    public class InvalidKeyException : Exception
    {
        public InvalidKeyException(string message) : base(message) { }
    }
}
