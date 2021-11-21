using CustomTowerDefense.Shared;

namespace CustomTowerDefense.GameObjects.SpaceShips
{
    public class SmallScoutShip: MoveableGameObject
    {
        private const int WIDTH = 60;
        private const int HEIGHT = 50;
        private const float SPEED = 10f;
        public const string ImagePathAndName = @"Sprites\Spaceship_0002_small";
        
        public SmallScoutShip(Coordinate coordinate):
            base(coordinate, WIDTH, HEIGHT, PreciseObjectType.SmallScoutShip, SPEED)
        {
            
        }
    }
}