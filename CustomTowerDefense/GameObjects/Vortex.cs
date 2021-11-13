using CustomTowerDefense.Shared;

namespace CustomTowerDefense.GameObjects
{
    public class Vortex: GameObject
    {
        private const int WIDTH = 64;
        private const int HEIGHT = 64;
        private const string IMAGE_PATH_AND_NAME = @"Sprites\vortex_64_64";
        
        public Vortex(Coordinate coordinate)
            : base(coordinate, WIDTH, HEIGHT, IMAGE_PATH_AND_NAME, PreciseObjectType.Vortex)
        {
        }
    }
}