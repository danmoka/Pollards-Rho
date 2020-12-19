using System.Numerics;

namespace PollardsRho
{
    public static class Helper
    {
        public static BigInteger ModInverse(BigInteger value, BigInteger modulo)
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
    }
}
