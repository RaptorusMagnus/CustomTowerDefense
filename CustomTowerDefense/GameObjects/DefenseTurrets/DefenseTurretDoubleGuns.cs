using CustomTowerDefense.Shared;

namespace CustomTowerDefense.GameObjects.DefenseTurrets
{
    public class DefenseTurretDoubleGuns: DefenseTurret
    {
        private const int WIDTH = 64;
        private const int HEIGHT = 64;
        private const int DRAW_ORDER = 10;
        private const ushort SIGHT_RANGE = 256;
        private const float ROTATION_SPEED = 0.01f; 
        
        public const string ImagePathAndName = @"Sprites\turet03_64";
        
        public DefenseTurretDoubleGuns(Coordinate coordinate)
            : base(coordinate,
                   WIDTH,
                   HEIGHT,
                   PreciseObjectType.DoubleGunsDefenseTurret,
                   DRAW_ORDER,
                   SIGHT_RANGE,
                   ROTATION_SPEED)
        {
        }
    }
}