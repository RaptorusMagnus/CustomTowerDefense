using CustomTowerDefense.Shared;

namespace CustomTowerDefense.GameObjects
{
    public class Vortex: GameObject
    {
        private const int WIDTH = 64;
        private const int HEIGHT = 64;
        private const int DRAW_ORDER = 0;
        public const string ImagePathAndName = @"Sprites\vortex_64_64";
        
        public Vortex(Coordinate coordinate)
            : base(coordinate, WIDTH, HEIGHT, PreciseObjectType.Vortex, DRAW_ORDER)
        {
        }
    }
}