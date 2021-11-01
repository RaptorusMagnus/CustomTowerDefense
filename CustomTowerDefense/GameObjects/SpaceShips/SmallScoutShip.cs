using CustomTowerDefense.ValueObjects;

namespace CustomTowerDefense.GameObjects.SpaceShips
{
    public class SmallScoutShip: MoveableGameObject
    {
        private const int SCOUT_WIDTH = 60;
        private const int SCOUT_HEIGHT = 50;
        private const float MAX_SPEED = 10f;
        private const string SCOUT_IMAGE_NAME = "Spaceship_0002_small";
        
        public SmallScoutShip(Coordinate coordinate):
            base(coordinate, SCOUT_WIDTH, SCOUT_HEIGHT, SCOUT_IMAGE_NAME, PreciseObjectType.SmallScoutShip, MAX_SPEED)
        {
            
        }
    }
}