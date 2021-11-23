using CustomTowerDefense.Shared;

namespace CustomTowerDefense.GameObjects
{
    public class DeffenseTurretDoubleGuns: GameObject
    {
        public const int WIDTH = 64;
        public const int HEIGHT = 64;
        public const string ImagePathAndName = @"Sprites\turet03_64";
        
        public DeffenseTurretDoubleGuns(Coordinate coordinate)
            : base(coordinate, WIDTH, HEIGHT, PreciseObjectType.DoubleGunsDefenseTurret)
        {
        }
    }
}