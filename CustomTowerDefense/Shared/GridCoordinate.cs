namespace CustomTowerDefense.Shared
{
    /// <summary>
    /// Simple immutable integer X-Y coordinate to be used for logical grid locations.
    /// </summary>
    public readonly struct GridCoordinate
    {
        public ushort X { get; }
        public ushort Y { get; }

        public GridCoordinate LeftSibling => new ((ushort)(X - 1), Y);
        public GridCoordinate RightSibling => new ((ushort)(X + 1), Y);
        public GridCoordinate TopSibling => new (X, (ushort)(Y - 1));
        public GridCoordinate BottomSibling => new (X, (ushort)(Y + 1));
        
        public GridCoordinate(ushort x, ushort y)
        {
            X = x;
            Y = y;
        }
        
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            
            if (obj.GetType() != typeof(GridCoordinate))
                return false;

            var receivedCoordinate = (GridCoordinate)obj;

            return this.X.Equals(receivedCoordinate.X) && this.Y.Equals(receivedCoordinate.Y);
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + X.GetHashCode();
            hash = (hash * 7) + Y.GetHashCode();

            return hash;
        }

        public override string ToString()
        {
            return $"[{X}, {Y}]";
        }
        
        public static bool operator ==(GridCoordinate left, GridCoordinate right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GridCoordinate left, GridCoordinate right)
        {
            return !(left == right);
        }
    }
}