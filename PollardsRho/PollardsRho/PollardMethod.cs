using System;
using System.Collections.Generic;

namespace PollardsRho
{
    public class PollardMethod
    {
        private readonly Random _random;
        private readonly Stack<(Point, int, int)> _tortoise;
        private readonly Stack<(Point, int, int)> _hare;

        private EllipticCurve _curve;
        private Point _x1;
        private Point _x2;
        private int _a1;
        private int _a2;
        private int _b1;
        private int _b2;

        public PollardMethod()
        {
            _random = new Random();
            _tortoise = new Stack<(Point, int, int)>();
            _hare = new Stack<(Point, int, int)>();
        }

        private Point X1 => _tortoise.Peek().Item1;
        private int A1 => _tortoise.Peek().Item2;
        private int B1 => _tortoise.Peek().Item3;
        private Point X2 => _hare.Peek().Item1;
        private int A2 => _hare.Peek().Item2;
        private int B2 => _hare.Peek().Item3;

        public int GetLogarithm(Point p, Point q, EllipticCurve curve)
        {
            InitValues(q, p, curve);

            if (!_curve.IsOnCurve(p) && !_curve.IsOnCurve(q))
                throw new Exception($"Points {p} or {q} are not on curve!");

            try
            {
                return GetLogarithm();
            }
            catch
            {
                return GetLogarithm(p, q, curve);
            }
        }

        private int GetLogarithm()
        {
            for (int i = 0; i < _curve.SubgroupOrder; i++)
            {
                Next(_tortoise);

                Next(_hare);
                Next(_hare);

                if (X1 == X2 && X1 != null && X2 != null)
                {
                    if (B1 == B2)
                        break;

                    var x = (A1 - A2) * Helper.ModInverse(B2 - B1, _curve.SubgroupOrder);
                    var logarithm = x % _curve.SubgroupOrder;

                    logarithm = Helper.GetValueByModulo(logarithm, _curve.SubgroupOrder);
                    ConsoleWorker.PrintAnswer(_tortoise, _hare, logarithm, _curve.SubgroupOrder);

                    return logarithm;
                }
            }

            throw new Exception("Логарифм не найден!");
        }

        private void Next(Stack<(Point, int, int)> current)
        {
            var x = current.Peek().Item1;
            var a = current.Peek().Item2;
            var b = current.Peek().Item3;

            var partitionSize = _curve.FieldOrder / 3 + 1;
            var i = x is null ? 0 : x.X / partitionSize;

            if (i == 0)
            {
                a += _a1;
                b += _b1;
                x = _curve.Add(x, _x1);
            }
            else if (i == 1)
            {
                a *= 2;
                b *= 2;
                x = _curve.DoubleUp(x);
            }
            else if (i == 2)
            {
                a += _a2;
                b += _b2;
                x = _curve.Add(x, _x2);
            }
            else
                throw new Exception(i.ToString());

            a %= _curve.SubgroupOrder;
            b %= _curve.SubgroupOrder;

            current.Push((x, a, b));
        }

        private void InitValues(Point p, Point q, EllipticCurve curve)
        {
            _curve = curve;

            _tortoise.Clear();
            _hare.Clear();

            _tortoise.Push(((Point)null, 0, 0));
            _hare.Push(((Point)null, 0, 0));

            _a1 = _random.Next(1, curve.SubgroupOrder);
            _b1 = _random.Next(1, curve.SubgroupOrder);
            _x1 = _curve.Add(
                _curve.Multiply(_a1, p),
                _curve.Multiply(_b1, q));

            _a2 = _random.Next(1, curve.SubgroupOrder);
            _b2 = _random.Next(1, curve.SubgroupOrder);
            _x2 = _curve.Add(
                _curve.Multiply(_a2, p),
                _curve.Multiply(_b2, q));
        }
    }
}
