using System.Globalization;
using System.Numerics;

namespace Eos.Cryptography
{
    public class EllipticCurve
    {
        static EllipticCurve()
        {
            BigInteger q, a, b, n;
            byte[] g;

            q = BigInteger.Parse("00FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFC2F", NumberStyles.AllowHexSpecifier);
            a = BigInteger.Zero;
            b = 7;
            n = BigInteger.Parse("00FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF48A03BBFD25E8CD0364141", NumberStyles.AllowHexSpecifier);

            SECP256K1 = new EllipticCurve()
            {
                Q = q,
                N = n
            };

            SECP256K1.A = new Field(a, SECP256K1);
            SECP256K1.B = new Field(b, SECP256K1);
            Field x = new Field("79BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798".FromHex().ToInt256(), SECP256K1);
            Field y = new Field("483ADA7726A3C4655DA4FBFC0E1108A8FD17B448A68554199C47D08FFB10D4B8".FromHex().ToInt256(), SECP256K1);
            SECP256K1.G = new Point(x, y, SECP256K1);
            SECP256K1.Infinity = new Point(null, null, SECP256K1);

            q = BigInteger.Parse("00FFFFFFFF00000001000000000000000000000000FFFFFFFFFFFFFFFFFFFFFFFF", NumberStyles.AllowHexSpecifier);
            a = BigInteger.Parse("00FFFFFFFF00000001000000000000000000000000FFFFFFFFFFFFFFFFFFFFFFFC", NumberStyles.AllowHexSpecifier);
            b = BigInteger.Parse("005AC635D8AA3A93E7B3EBBD55769886BC651D06B0CC53B0F63BCE3C3E27D2604B", NumberStyles.AllowHexSpecifier);
            n = BigInteger.Parse("00FFFFFFFF00000000FFFFFFFFFFFFFFFFBCE6FAADA7179E84F3B9CAC2FC632551", NumberStyles.AllowHexSpecifier);

            SECP256R1 = new EllipticCurve()
            {
                Q = q,
                N = n
            };

            SECP256R1.A = new Field(a, SECP256R1);
            SECP256R1.B = new Field(b, SECP256R1);
            x = new Field("6B17D1F2E12C4247F8BCE6E563A440F277037D812DEB33A0F4A13945D898C296".FromHex().ToInt256(), SECP256R1);
            y = new Field("4FE342E2FE1A7F9B8EE7EB4A7C0F9E162BCE33576B315ECECBB6406837BF51F5".FromHex().ToInt256(), SECP256R1);
            SECP256R1.G = new Point(x, y, SECP256R1);
            SECP256R1.Infinity = new Point(null, null, SECP256R1);
        }

        public static EllipticCurve SECP256K1 { get; private set; }

        public static EllipticCurve SECP256R1 { get; private set; }

        public BigInteger Q { get; private set; }

        public Field A { get; private set; }

        public Field B { get; private set; }

        public BigInteger N { get; private set; }

        public Point Infinity { get; private set; }

        public Point G { get; private set; }   
    }
}
