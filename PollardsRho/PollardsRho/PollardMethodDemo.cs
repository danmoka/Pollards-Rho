namespace PollardsRho
{
    public class PollardMethodDemo
    {
        private readonly PollardMethod _pollardMethod;

        public PollardMethodDemo()
        {
            _pollardMethod = new PollardMethod();
        }

        public void Run(int number = 0)
        {
            if (number == 1)
                GetLogarithm(GetValues_1());
            else if (number == 2)
                GetLogarithm(GetValues_2());
            else 
                GetLogarithm(InputValues());
        }

        private void GetLogarithm((Point, Point, EllipticCurve) values)
        {
            var p = values.Item1;
            var q = values.Item2;
            var curve = values.Item3;

            _pollardMethod.GetLogarithm(p, q, curve);
        }

        private (Point, Point, EllipticCurve) InputValues()
        {
            return ConsoleWorker.InputValues();
        }

        private (Point, Point, EllipticCurve) GetValues_1()
        {
            var p = new Point(80, 87);
            var q = new Point(3, 6);
            var curve = new EllipticCurve(2, 3, 97, 5, p);

            ConsoleWorker.PrintValues(p, q, curve);

            return (p, q, curve);
        }

        private (Point, Point, EllipticCurve) GetValues_2()
        {
            var p = new Point(24, 53);
            var q = new Point(31, 6);
            var curve = new EllipticCurve(3, 9, 73, 83, p);

            ConsoleWorker.PrintValues(p, q, curve);

            return (p, q, curve);
        }
    }
}
