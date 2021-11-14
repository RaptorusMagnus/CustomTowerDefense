using CustomTowerDefense.Shared;

namespace CustomTowerDefense.GameObjects
{
    public class StructureElement: GameObject
    {
        private const int WIDTH = 64;
        private const int HEIGHT = 64;
        private const string IMAGE_NAME = "StructureElement_04";
        
        public StructureElement(Coordinate coordinate) :
            base(coordinate, WIDTH, HEIGHT, IMAGE_NAME, PreciseObjectType.StructureElement)
        {
        }
    }
}