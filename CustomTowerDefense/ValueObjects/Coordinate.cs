namespace CustomTowerDefense.ValueObjects
{
    /// <summary>
    /// Simple immutable X-Y coordinate
    /// </summary>
    public struct Coordinate
    {
        public int X { get; }
        public int Y { get; }

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Coordinate))
                return false;

            Coordinate receivedCoordinate = (Coordinate)obj;

            return this.X == receivedCoordinate.X && this.Y == receivedCoordinate.Y;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + X.GetHashCode();
            hash = (hash * 7) + Y.GetHashCode();

            return hash;
        }
    }
}