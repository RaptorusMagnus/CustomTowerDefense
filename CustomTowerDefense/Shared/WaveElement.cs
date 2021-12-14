using CustomTowerDefense.GameObjects.SpaceShips;

namespace CustomTowerDefense.Shared
{
    /// <summary>
    /// One element in an attack wave
    /// </summary>
    public class WaveElement
    {
        /// <summary>
        /// Delay before creating the spaceship.
        /// A delay equal to zero will spawn spaceships at vortex speed.
        /// In other words, if you don't want to make an explicit pause, you can leave the delay to zero.
        /// </summary>
        public ushort DelayBeforeCreation { get; set; }
        
        /// <summary>
        /// The spaceship to create.
        /// </summary>
        public SpaceShip SpaceShip { get; set; }
    }
}