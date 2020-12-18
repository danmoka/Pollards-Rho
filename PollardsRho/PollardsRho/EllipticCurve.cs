using System;
using System.Collections.Generic;
using System.Numerics;

namespace PollardsRho
{
    public class EllipticCurve
    {
        private readonly BigInteger _a;
        private readonly BigInteger _b;
        private readonly BigInteger _fieldOrder;
        private readonly BigInteger _subgroupOrder;
        public Point BasePoint { get; private set; }

        public EllipticCurve(
            BigInteger a,
            BigInteger b,
            BigInteger fieldOrder,
            BigInteger subgroupOrder,
            Point basePoint)
        {
            _a = a;
            _b = b;
            _fieldOrder = fieldOrder;
            _subgroupOrder = subgroupOrder;
            BasePoint = basePoint;

            if (BigInteger.ModPow(2, _fieldOrder - 1, _fieldOrder) != 1)
                throw new InvalidOperationException($"Incorrect field order {_fieldOrder}!");

            if (CurveDiscriminant() == 0)
                throw new InvalidOperationException("The discriminant is equal to zero!");

            if (!IsOnCurve(BasePoint))
                throw new InvalidOperationException($"The base point {BasePoint} is not on the curve!");

            if (Multiply(_subgroupOrder, BasePoint) != null)
                throw new InvalidOperationException($"Incorrect {_subgroupOrder} or {BasePoint}!");
        }

        public Point Add(Point p1, Point p2)
        {
            if (!IsOnCurve(p1))
                throw new ArgumentException($"Point {p1} is not on the curve!");

            if (!IsOnCurve(p2))
                throw new ArgumentException($"Point {p2} is not on the curve!");

            if (p1 == null)
                return p2;

            if (p2 == null)
                return p1;

            // Если прямая параллельна оси ординат, третьей точкой будет точка в бесконечности
            if (p1.X == p2.X && p1.Y != p2.Y)
                return null;

            BigInteger s;

            if (p1.X == p2.X)
                s = ((3 * ModPow(p1.X, 2) + _a) * ModInverse(2 * p1.Y, _fieldOrder)) % _fieldOrder;
            else
                s = ((p1.Y - p2.Y) * ModInverse(p1.X - p2.X, _fieldOrder)) % _fieldOrder;

            var x = (ModPow(s, 2) - p1.X - p2.X) % _fieldOrder;

            if (x < 0)
                x += _fieldOrder;

            var y = (p1.Y + s * (x - p1.X)) % _fieldOrder;

            y = -y < 0 ? 
                -y + _fieldOrder :
                -y;

            var sumPoint = new Point(x, y);

            if (!IsOnCurve(sumPoint))
                throw new InvalidOperationException($"Point {sumPoint} is not on curve!");

            return sumPoint;
        }

        public Point DoubleUp(Point p)
        {
            return Add(p, p);
        }

        public Point NegX_axisPoint(Point p)
        {
            if (p == null)
                return null;

            var y = -p.Y % _fieldOrder;
            y = y < 0 ? 
                y + _fieldOrder : 
                y;

            var negPoint = new Point(p.X, y);

            if (!IsOnCurve(negPoint))
                throw new InvalidOperationException($"Point {negPoint} is not on curve!");

            return negPoint;
        }

        public BigInteger ModInverse(BigInteger value, BigInteger modulo)
        {
            if (modulo == 1) 
                return 0;

            var m0 = modulo;
            BigInteger x = 1;
            BigInteger y = 0;

            if (value < 0) 
                return modulo - ModInverse(-value, modulo);

            while (value > 1)
            {
                var q = value / modulo;
                (value, modulo) = (modulo, value % modulo);
                (x, y) = (y, x - q * y);
            }

            return x < 0 ? x + m0 : x;
        }

        //private BigInteger ModInverse(BigInteger value, BigInteger modulo)
        //{
        //    BigInteger x, y;

        //    if (1 != EGCD(value, modulo, out x, out y))
        //        throw new ArgumentException("Invalid modulo", "modulo");

        //    if (x < 0)
        //        x += modulo;

        //    return x % modulo;
        //}

        public bool IsOnCurve(Point p)
        {
            if (p == null)
                return true;

            return (ModPow(p.Y, 2) - ModPow(p.X, 3) - _a * p.X - _b) % _fieldOrder == 0;
        }

        public Point Multiply(BigInteger n, Point p)
        {
            if (n % _subgroupOrder == 0 || p == null)
                return null;

            if (n < 0)
                return NegX_axisPoint(Multiply(-n, p));

            Point multiplyPoint = null;
            var copy = p.Copy();

            while (n != 0)
            {
                if ((n & 1) != 0)
                {
                    multiplyPoint = Add(multiplyPoint, copy);
                }

                copy = DoubleUp(copy);
                n >>= 1;
            }

            // можно не проверять, что точка на кривой, т.к. это проверится в Add

            return multiplyPoint;
        }

        private BigInteger CurveDiscriminant()
        {
            return -16 * (4 * ModPow(_a, 3) + 27 * ModPow(_b, 2)) % _fieldOrder;
        }

        private BigInteger ModPow(BigInteger a, BigInteger n)
        {
            return BigInteger.ModPow(a, n, _fieldOrder);
        }

        //private BigInteger EGCD(BigInteger left,
        //                      BigInteger right,
        //                  out BigInteger leftFactor,
        //                  out BigInteger rightFactor)
        //{
        //    leftFactor = 0;
        //    rightFactor = 1;
        //    BigInteger u = 1;
        //    BigInteger v = 0;
        //    BigInteger gcd = 0;

        //    while (left != 0)
        //    {
        //        BigInteger q = right / left;
        //        BigInteger r = right % left;

        //        BigInteger m = leftFactor - u * q;
        //        BigInteger n = rightFactor - v * q;

        //        right = left;
        //        left = r;
        //        leftFactor = u;
        //        rightFactor = v;
        //        u = m;
        //        v = n;

        //        gcd = right;
        //    }

        //    return gcd;
        //}
    }
}
