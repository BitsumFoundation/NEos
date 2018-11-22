using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace NEos.Cryptography
{
    public class Point
    {
        public Point(Field x, Field y, EllipticCurve curve)
        {
            X = x;
            Y = y;
            Curve = curve;
        }

        public Field X { get; private set; }

        public Field Y { get; private set; }

        public EllipticCurve Curve { get; private set; }

        public bool IsInfinity
        {
            get { return X == null && Y == null; }
        }
        
        public static Point operator -(Point x)
        {
            return new Point(x.X, -x.Y, x.Curve);
        }

        public static Point operator +(Point x, Point y)
        {
            if (x.IsInfinity) return y;

            if (y.IsInfinity) return x;

            if (x.X.Equals(y.X))
            {
                if (x.Y.Equals(y.Y)) return x.Twice();

                Debug.Assert(x.Y.Equals(-y.Y));

                return x.Curve.Infinity;
            }

            Field gamma = (y.Y - x.Y) / (y.X - x.X);
            Field x3 = gamma.Square() - x.X - y.X;
            Field y3 = gamma * (x.X - x3) - x.Y;

            return new Point(x3, y3, x.Curve);
        }

        public static Point operator -(Point x, Point y)
        {
            if (y.IsInfinity) return x;

            return x + (-y);
        }

        public static Point operator *(Point x, BigInteger k)
        {
            int m = k.GetBitCount();
            sbyte width;
            int reqPreCompLen;

            if (m < 13)
            {
                width = 2;
                reqPreCompLen = 1;
            }
            else if (m < 41)
            {
                width = 3;
                reqPreCompLen = 2;
            }
            else if (m < 121)
            {
                width = 4;
                reqPreCompLen = 4;
            }
            else if (m < 337)
            {
                width = 5;
                reqPreCompLen = 8;
            }
            else if (m < 897)
            {
                width = 6;
                reqPreCompLen = 16;
            }
            else if (m < 2305)
            {
                width = 7;
                reqPreCompLen = 32;
            }
            else
            {
                width = 8;
                reqPreCompLen = 127;
            }

            int preCompLen = 1;

            Point[] preComp = preComp = new Point[] { x };
            Point twiceP = x.Twice();

            if (preCompLen < reqPreCompLen)
            {
                Point[] oldPreComp = preComp;
                preComp = new Point[reqPreCompLen];
                Array.Copy(oldPreComp, 0, preComp, 0, preCompLen);

                for (int i = preCompLen; i < reqPreCompLen; i++)
                {
                    preComp[i] = twiceP + preComp[i - 1];
                }
            }

            sbyte[] wnaf = WindowNaf(width, k);
            int l = wnaf.Length;

            Point q = x.Curve.Infinity;

            for (int i = l - 1; i >= 0; i--)
            {
                q = q.Twice();

                if (wnaf[i] != 0)
                {
                    if (wnaf[i] > 0)
                    {
                        q += preComp[(wnaf[i] - 1) / 2];
                    }
                    else
                    {
                        q -= preComp[(-wnaf[i] - 1) / 2];
                    }
                }
            }

            return q;
        }

        public byte[] Encode()
        {
            byte[] res = new byte[33];

            byte[] buf = X.Value.GetBytes();

            Array.Copy(buf, 0, res, 33 - buf.Length, buf.Length);

            res[0] = Y.Value.IsEven ? (byte)2 : (byte)3;

            return res;
        }

        public Point Twice()
        {
            if (this.IsInfinity) return this;

            if (this.Y.Value.Sign == 0) return Curve.Infinity;

            Field two = new Field(2, Curve);
            Field three = new Field(3, Curve);
            Field gamma = (this.X.Square() * three + Curve.A) / (Y * two);
            Field x3 = gamma.Square() - this.X * two;
            Field y3 = gamma * (this.X - x3) - this.Y;

            return new Point(x3, y3, Curve);
        }

        public Point MultiplyTwo(BigInteger j, Point x, BigInteger k)
        {
            var i = Math.Max(j.GetBitCount(), k.GetBitCount()) - 1;
            var r = Curve.Infinity;
            var both = this + x;
            while (i >= 0)
            {
                var jBit = j.TestBit(i);
                var kBit = k.TestBit(i);

                r = r.Twice();

                if (jBit)
                {
                    if (kBit)
                    {
                        r = r + both;
                    }
                    else
                    {
                        r = r + this;
                    }
                }
                else
                {
                    if (kBit)
                    {
                        r = r + x;
                    }
                }

                --i;
            }

            return r;
        }

        public static Point FromX(BigInteger x, bool isOdd, EllipticCurve curve)
        {
            var pOverFour = curve.Q + BigInteger.One >> 2;
            var alpha = (BigInteger.Pow(x, 3) + curve.A.Value * x + curve.B.Value).Mod(curve.Q);
            var beta = BigInteger.ModPow(alpha, pOverFour, curve.Q);

            var y = beta;
            if (beta.IsEven ^ !isOdd)
            {
                y = curve.Q - y;
            }

            return new Point(new Field(x, curve), new Field(y, curve), curve);
        }



        private static sbyte[] WindowNaf(sbyte width, BigInteger k)
        {
            sbyte[] wnaf = new sbyte[k.GetBitCount() + 1];
            short pow2wB = (short)(1 << width);
            int i = 0;
            int length = 0;

            while (k.Sign > 0)
            {
                if (!k.IsEven)
                {
                    BigInteger remainder = k % pow2wB;
                    if (remainder.TestBit(width - 1))
                    {
                        wnaf[i] = (sbyte)(remainder - pow2wB);
                    }
                    else
                    {
                        wnaf[i] = (sbyte)remainder;
                    }

                    k -= wnaf[i];
                    length = i;
                }
                else
                {
                    wnaf[i] = 0;
                }

                k >>= 1;
                i++;
            }

            length++;

            sbyte[] wnafShort = new sbyte[length];
            Array.Copy(wnaf, 0, wnafShort, 0, length);

            return wnafShort;
        }
    }
}
