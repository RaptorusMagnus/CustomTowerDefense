using Microsoft.Xna.Framework;

namespace CustomTowerDefense.Shared
{
    /// <summary>
    /// Simple immutable X-Y coordinate.
    /// X and Y must be floats because for very slow moving objects we could have tiny position increments.
    /// This way, the position changes little by little until it reaches the next pixel. 
    /// </summary>
    public struct Coordinate
    {
        public float X { get; }
        public float Y { get; }

        public Coordinate LeftSibling => new Coordinate(X - 1, Y);
        public Coordinate RightSibling => new Coordinate(X + 1, Y);
        public Coordinate TopSibling => new Coordinate(X, Y - 1);
        public Coordinate BottomSibling => new Coordinate(X, Y + 1);
        
        public Coordinate(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2 GetVector2()
        {
            return new Vector2(X, Y);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            
            if (obj.GetType() != typeof(Coordinate))
                return false;

            var receivedCoordinate = (Coordinate)obj;

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
    }
}