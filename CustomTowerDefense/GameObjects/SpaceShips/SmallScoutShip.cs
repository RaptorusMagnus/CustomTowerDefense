using System.Collections.Generic;
using CustomTowerDefense.GameGrids;
using CustomTowerDefense.Shared;

namespace CustomTowerDefense.GameObjects.SpaceShips
{
    public class SmallScoutShip: SpaceShip
    {
        private const int WIDTH = 60;
        private const int HEIGHT = 50;
        private const float SPEED = 1.5f;
        public const string ImagePathAndName = @"Sprites\Spaceship_0002_small";
        
        public SmallScoutShip(Coordinate coordinate, List<GridCoordinate> path, LogicalGameGridMultiple logicalGameGrid):
            base(coordinate, WIDTH, HEIGHT, PreciseObjectType.SmallScoutShip, SPEED, path, logicalGameGrid)
        {
            
        }
    }
}