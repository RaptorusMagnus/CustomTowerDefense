using CustomTowerDefense.Shared;

namespace CustomTowerDefense.GameObjects
{
    public class DeffenseTurretDoubleGuns: GameObject
    {
        private const int WIDTH = 64;
        private const int HEIGHT = 64;
        private const int DRAW_ORDER = 10;
        public const string ImagePathAndName = @"Sprites\turet03_64";
        
        public DeffenseTurretDoubleGuns(Coordinate coordinate)
            : base(coordinate, WIDTH, HEIGHT, PreciseObjectType.DoubleGunsDefenseTurret, DRAW_ORDER)
        {
        }
    }
}