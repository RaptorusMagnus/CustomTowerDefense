using System.Collections.Generic;
using CustomTowerDefense.GameGrids;
using CustomTowerDefense.Shared;

namespace CustomTowerDefense.GameObjects.SpaceShips
{
    public class SmallScoutShip: SpaceShip
    {
        private const int WIDTH = 60;
        private const int HEIGHT = 50;
        private const int DRAW_ORDER = 20;
        private const float SPEED = 1.5f;
        private const ushort HIT_POINTS = 100;
        public const string ImagePathAndName = @"Sprites\Spaceship_0002_small";
        
        public SmallScoutShip(Coordinate coordinate, List<GridCoordinate> path, LogicalGameGridMultiple logicalGameGrid):
            base(coordinate,
                WIDTH,
                HEIGHT,
                PreciseObjectType.SmallScoutShip,
                SPEED,
                DRAW_ORDER,
                HIT_POINTS,
                path,
                logicalGameGrid)
        {
        }
    }
}