using System;

namespace NEos.Cryptography
{
    public class InvalidSignatureException : Exception
    {
        public InvalidSignatureException(string message) : base(message) { }
    }
}
