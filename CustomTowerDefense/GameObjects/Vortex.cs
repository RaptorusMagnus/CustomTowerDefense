using CustomTowerDefense.Shared;

namespace CustomTowerDefense.GameObjects
{
    public class Vortex: GameObject
    {
        private const int WIDTH = 64;
        private const int HEIGHT = 64;
        private const int DRAW_ORDER = 0;
        
        /// <summary>
        /// Minimum rotation speed for a vortex.
        /// We must set physical limits to avoid problems with custom levels and waves created by external users.
        /// </summary>
        public const float MIN_ROTATION_SPEED = 0.01f;
        
        /// <summary>
        /// Maximum rotation speed for a vortex.
        /// We must set physical limits to avoid problems with custom levels and waves created by external users.
        /// </summary>
        public const float MAX_ROTATION_SPEED = 0.01f;
        
        public const string ImagePathAndName = @"Sprites\vortex_64_64";
        
        public Vortex(Coordinate coordinate)
            : base(coordinate, WIDTH, HEIGHT, PreciseObjectType.Vortex, DRAW_ORDER)
        {
        }
    }
}