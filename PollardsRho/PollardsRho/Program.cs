using System;

namespace PollardsRho
{
    class Program
    {
        static void Main(string[] args)
        {
            var basePoint = new Point(17, 10);
            var curve = new EllipticCurve(2, 3, 97, 100, basePoint);
            var pollardMethod = new PollardMethod();

            var p = new Point(3, 6);
            var q = new Point(80, 87);
            var log = pollardMethod.Log(p, q, curve);

            Console.WriteLine(log);
            Console.ReadLine();
        }
    }
}
