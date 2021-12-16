using System;
using CustomTowerDefense.GameObjects.SpaceShips;

namespace CustomTowerDefense.GameEngine
{
    /// <summary>
    /// One element in an attack wave
    /// </summary>
    public class WaveElement
    {
        private Type _spaceshipType;
        
        /// <summary>
        /// Delay before creating the spaceship (milliseconds).
        /// A delay equal to zero will spawn spaceships at vortex speed.
        /// In other words, if you don't want to make an explicit pause, you can leave the delay to zero.
        /// </summary>
        public ushort DelayBeforeCreation { get; set; }
        
        /// <summary>
        /// The spaceship to create.
        /// </summary>
        public Type SpaceShipType
        {
            get => _spaceshipType;
            set
            {
                if (value.IsSubclassOf(typeof(SpaceShip)))
                    _spaceshipType = value;
                else
                    throw new Exception($"The received type '{value}' is not a valid spaceship type.");
            }
        }
    }
}