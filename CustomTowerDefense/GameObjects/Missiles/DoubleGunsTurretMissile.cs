using CustomTowerDefense.GameGrids;
using CustomTowerDefense.Shared;

namespace CustomTowerDefense.GameObjects.Missiles
{
    public class DoubleGunsTurretMissile: Missile
    {
        private const int WIDTH = 16;
        private const int HEIGHT = 6;
        private const int DRAW_ORDER = 21;
        private const float SPEED = 8f;
        private const ushort DAMAGE_POINTS = 20;
        public const string ImagePathAndName = @"Sprites\double-gun-orange-fire";
        
        public DoubleGunsTurretMissile(Coordinate coordinate, LogicalGameGridMultiple gameGrid)
            : base(coordinate,
                   WIDTH,
                   HEIGHT,
                   PreciseObjectType.DoubleGunsDefenseTurretFire,
                   SPEED,
                   DRAW_ORDER,
                   DAMAGE_POINTS,
                   gameGrid)
        {
        }
    }
}