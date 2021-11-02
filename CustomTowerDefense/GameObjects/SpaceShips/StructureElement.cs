using CustomTowerDefense.ValueObjects;

namespace CustomTowerDefense.GameObjects.SpaceShips
{
    public class StructureElement: GameObject
    {
        private const int WIDTH = 37;
        private const int HEIGHT = 37;
        private const string IMAGE_NAME = "StructureElement_04";
        
        public StructureElement(Coordinate coordinate) :
            base(coordinate, WIDTH, HEIGHT, IMAGE_NAME, PreciseObjectType.StructureElement)
        {
        }
    }
}