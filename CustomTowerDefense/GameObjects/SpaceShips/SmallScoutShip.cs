using CustomTowerDefense.ValueObjects;

namespace CustomTowerDefense.GameObjects.SpaceShips
{
    public class SmallScoutShip: MoveableGameObject
    {
        private const int WIDTH = 60;
        private const int HEIGHT = 50;
        private const float SPEED = 10f;
        private const string IMAGE_PATH_AND_NAME = @"Sprites\Spaceship_0002_small";
        
        public SmallScoutShip(Coordinate coordinate):
            base(coordinate, WIDTH, HEIGHT, IMAGE_PATH_AND_NAME, PreciseObjectType.SmallScoutShip, SPEED)
        {
            
        }
    }
}