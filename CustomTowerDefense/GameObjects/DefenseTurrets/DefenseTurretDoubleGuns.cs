using CustomTowerDefense.GameGrids;
using CustomTowerDefense.Shared;

namespace CustomTowerDefense.GameObjects.DefenseTurrets
{
    public class DefenseTurretDoubleGuns: DefenseTurret, IBuyable
    {
        private const int WIDTH = 64;
        private const int HEIGHT = 64;
        private const int DRAW_ORDER = 10;
        private const ushort SIGHT_RANGE = 256;
        private const float ROTATION_SPEED = 0.01f;
        private const ushort FIRING_DELAY = 600;

        /// <inheritdoc cref="IBuyable.SalePrice"/>
        public const ushort SalePrice = 50;
        
        public const string ImagePathAndName = @"Sprites\turet03_64";
        
        public DefenseTurretDoubleGuns(Coordinate coordinate, LogicalGameGridMultiple logicalGameGrid)
            : base(coordinate,
                   WIDTH,
                   HEIGHT,
                   PreciseObjectType.DoubleGunsDefenseTurret,
                   DRAW_ORDER,
                   SIGHT_RANGE,
                   ROTATION_SPEED,
                   FIRING_DELAY,
                   SalePrice,
                   logicalGameGrid)
        {
        }
    }
}