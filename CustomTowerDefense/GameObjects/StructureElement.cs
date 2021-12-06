using CustomTowerDefense.Shared;

namespace CustomTowerDefense.GameObjects
{
    public class StructureElement: GameObject
    {
        private const int WIDTH = 64;
        private const int HEIGHT = 64;
        private const int DRAW_ORDER = 0;
        public const string ImagePathAndName = @"Sprites\StructureElementGrayscale2";
        
        public StructureElement(Coordinate coordinate) :
            base(coordinate, WIDTH, HEIGHT, PreciseObjectType.StructureElement, DRAW_ORDER)
        {
        }
    }
}