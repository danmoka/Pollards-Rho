using System;
using System.Numerics;

namespace PollardsRho
{
    class Program
    {
        static void Main(string[] args)
        {
            var basePoint = new Point(31, 6);
            var curve = new EllipticCurve(3, 9, 73, 83, basePoint);
            var pollardMethod = new PollardMethod();

            var p = new Point(31, 6);
            var q = new Point(24, 53);
            var log = pollardMethod.Log(p, q, curve);

            Console.WriteLine(log);
            // это я тыкал возведение в степень и как получать по модулю
            var  i = int.MaxValue;
            Console.WriteLine((i + 2147000000) % 73);
            Console.WriteLine((i + 2147000000) % 73 + 73);
            var v = ((int) BigInteger.ModPow(2, 72, 73));

            Console.WriteLine(v);
            Console.ReadLine();
        }
    }
}
