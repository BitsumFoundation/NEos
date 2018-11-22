using System;
using System.Numerics;

namespace Eos.Cryptography
{
    public class Field 
    {
        public Field(BigInteger value, EllipticCurve curve)
        {
            if (value >= curve.Q)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            Value = value;
            Curve = curve;
        }

        public BigInteger Value { get; private set; }

        public EllipticCurve Curve { get; private set; }

        public Field Square()
        {
            return new Field((Value * Value).Mod(Curve.Q), Curve);
        }

        public static Field operator -(Field x)
        {
            return new Field((-x.Value).Mod(x.Curve.Q), x.Curve);
        }

        public static Field operator *(Field x, Field y)
        {
            return new Field((x.Value * y.Value).Mod(x.Curve.Q), x.Curve);
        }

        public static Field operator /(Field x, Field y)
        {
            return new Field((x.Value * y.Value.ModInverse(x.Curve.Q)).Mod(x.Curve.Q), x.Curve);
        }

        public static Field operator +(Field x, Field y)
        {
            return new Field((x.Value + y.Value).Mod(x.Curve.Q), x.Curve);
        }

        public static Field operator -(Field x, Field y)
        {
            return new Field((x.Value - y.Value).Mod(x.Curve.Q), x.Curve);
        }
    }
}
