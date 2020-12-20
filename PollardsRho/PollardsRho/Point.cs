namespace PollardsRho
{
    public class Point
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point Copy()
        {
            return new Point(X, Y);
        }

        public override bool Equals(object obj)
        {
            if (obj is Point point)
            {
                return X == point.X && Y == point.Y;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode();
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public static bool operator ==(Point p1, Point p2)
        {
            if (ReferenceEquals(p1, null))
            {
                if (ReferenceEquals(p2, null))
                {
                    return true;
                }

                return false;
            }

            return p1.Equals(p2);
        }

        public static bool operator !=(Point p1, Point p2)
        {
            return !(p1 == p2);
        }
    }
}
