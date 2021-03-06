﻿using System;

namespace PollardsRho
{
    public class EllipticCurve
    {
        private readonly int _a;
        private readonly int _b;
        private readonly int _fieldOrder;
        private readonly int _subgroupOrder;

        public EllipticCurve(
            int a,
            int b,
            int fieldOrder,
            int subgroupOrder,
            Point basePoint)
        {
            _a = a;
            _b = b;
            _fieldOrder = fieldOrder;
            _subgroupOrder = subgroupOrder;

            A = a;
            B = b;
            BasePoint = basePoint;
            FieldOrder = fieldOrder;
            SubgroupOrder = subgroupOrder;

            if (ModPow(2, _fieldOrder - 1) != 1)
                throw new InvalidOperationException($"Incorrect field order {_fieldOrder}!");

            if (CurveDiscriminant() == 0)
                throw new InvalidOperationException("The discriminant is equal to zero!");

            if (!IsOnCurve(BasePoint))
                throw new InvalidOperationException($"The base point {BasePoint} is not on the curve!");

            if (Multiply(_subgroupOrder, BasePoint) != null)
                throw new InvalidOperationException($"Incorrect {_subgroupOrder} or {BasePoint}!");
        }

        public int A { get; private set; }
        public int B { get; private set; }
        public Point BasePoint { get; private set; }
        public int FieldOrder { get; private set; }
        public int SubgroupOrder { get; private set; }

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

            int s;

            if (p1.X == p2.X)
                s = Mod((3 * ModPow(p1.X, 2) + _a) * Helper.ModInverse(2 * p1.Y, _fieldOrder));
            else
                s = Mod((p1.Y - p2.Y) * Helper.ModInverse(p1.X - p2.X, _fieldOrder));

            var x = Mod(ModPow(s, 2) - p1.X - p2.X);

            var y = Mod(p1.Y + s * (x - p1.X));

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

            var y = Mod(-p.Y % _fieldOrder);
            var negPoint = new Point(p.X, y);

            if (!IsOnCurve(negPoint))
                throw new InvalidOperationException($"Point {negPoint} is not on curve!");

            return negPoint;
        }

        public bool IsOnCurve(Point p)
        {
            if (p == null)
                return true;

            return Mod(ModPow(p.Y, 2) - ModPow(p.X, 3) - _a * p.X - _b) == 0;
        }

        public Point Multiply(int n, Point p)
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

        private int CurveDiscriminant()
        {
            return Mod(-16 * (4 * ModPow(_a, 3) + 27 * ModPow(_b, 2)));
        }

        private int ModPow(int a, int n)
        {
            var result = Math.Pow(a, n) % _fieldOrder;
            result = result < 0
                ? result + _fieldOrder
                : result;

            return (int) result;
        }

        private int Mod(int a)
        {
            var result = a % _fieldOrder;
            result = result < 0
                ? result + _fieldOrder
                : result;

            return result;
        }
    }
}
