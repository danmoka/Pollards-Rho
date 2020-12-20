namespace PollardsRho
{
    public static class Helper
    {
        public static int ModInverse(int value, int modulo)
        {
            if (modulo == 1)
                return 0;

            var m0 = modulo;
            var x = 1;
            var y = 0;

            if (value < 0)
                return modulo - ModInverse(-value, modulo);

            while (value > 1)
            {
                var q = value / modulo;
                (value, modulo) = (modulo, value % modulo);
                (x, y) = (y, x - q * y);
            }

            return x < 0 
                ? x + m0 
                : x;
        }

        public static int GetValueByModulo(int value, int modulo)
        {
            return value >= 0
                ? value
                : value + modulo;
        }
    }
}
