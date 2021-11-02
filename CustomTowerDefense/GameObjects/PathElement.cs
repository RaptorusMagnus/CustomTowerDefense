using CustomTowerDefense.ValueObjects;

namespace CustomTowerDefense.GameObjects
{
    /// <summary>
    /// One element of the path that will be followed by the space ships to reach the final target.
    /// </summary>
    public class PathElement: GameObject
    {
        private const int WIDTH = 37;
        private const int HEIGHT = 37;
        private const string IMAGE_NAME = "PathElement";
        
        public PathElement(Coordinate coordinate) :
            base(coordinate, WIDTH, HEIGHT, IMAGE_NAME, PreciseObjectType.PathElement)
        {
        }
    }
}