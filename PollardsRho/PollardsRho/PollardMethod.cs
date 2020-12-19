using System;
using System.Collections.Generic;

namespace PollardsRho
{
    public class PollardMethod
    {
        private readonly Random _random;

        private Point _point1;
        private Point _point2;
        private int _a1;
        private int _a2;
        private int _b1;
        private int _b2;
        private Point _x1;
        private Point _x2;
        private EllipticCurve _curve;

        public PollardMethod()
        {
            _random = new Random();
        }

        public int Log(Point p, Point q, EllipticCurve curve)
        {
            InitValues(p, q, curve);

            if (!_curve.IsOnCurve(p) && !_curve.IsOnCurve(q))
                throw new Exception($"Points {p} or {q} are not on curve!");

            for (int i = 0; i < 3; i++)
            {
                var tortoise = ((Point)null, 0, 0);
                var hare = ((Point)null, 0, 0);

                for (int j = 0; j < _curve.SubgroupOrder; j++)
                {
                    var tup1 = Iter(tortoise);

                    var tup2 = Iter(hare);
                    tup2 = Iter(tup2);

                    if (tup1.Item1 == tup2.Item1)
                    {
                        if (tup1.Item3 == tup2.Item3)
                            break;

                        var x = (tup1.Item2 - tup2.Item2) * Helper.ModInverse(tup2.Item3 - tup1.Item3, _curve.SubgroupOrder);
                        var logarithm = x % _curve.SubgroupOrder;

                        return logarithm;
                    }
                }
            }

            return 0;
        }

        private (Point, int, int) Iter((Point x, int a, int b) tuple)
        {
            var partitionSize = _curve.FieldOrder / 3 + 1;
            var newTuple = (tuple.x, tuple.a, tuple.b);

            int i;

            if (tuple.x is null)
                i = 0;
            else
                i = newTuple.x.X / partitionSize;

            if (i == 0)
            {
                newTuple.a += _a1;
                newTuple.b += _b1;
                newTuple.x = _curve.Add(newTuple.x, _x1);
            }
            else if (i == 1)
            {
                newTuple.a *= 2;
                newTuple.b *= 2;
                newTuple.x = _curve.DoubleUp(newTuple.x);
            }
            else if (i == 2)
            {
                newTuple.a += _a2;
                newTuple.b += _b2;
                newTuple.x = _curve.Add(newTuple.x, _x2);
            }
            else
                throw new Exception(i.ToString());

            newTuple.a %= _curve.SubgroupOrder;
            newTuple.b %= _curve.SubgroupOrder;

            return newTuple;
        }

        private void InitValues(Point p, Point q, EllipticCurve curve)
        {
            _curve = curve;

            _point1 = p;
            _point2 = q;

            _a1 = _random.Next(1, (int)curve.SubgroupOrder);
            _b1 = _random.Next(1, (int)curve.SubgroupOrder);
            _x1 = _curve.Add(
                _curve.Multiply(_a1, _point1),
                _curve.Multiply(_b1, _point2));

            _a2 = _random.Next(1, (int)curve.SubgroupOrder);
            _b2 = _random.Next(1, (int)curve.SubgroupOrder);
            _x2 = _curve.Add(
                _curve.Multiply(_a2, _point1),
                _curve.Multiply(_b2, _point2));
        }
    }
}
