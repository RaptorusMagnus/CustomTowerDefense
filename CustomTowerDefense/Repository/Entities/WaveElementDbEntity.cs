using System;
using CustomTowerDefense.GameEngine;
using CustomTowerDefense.GameObjects;
using CustomTowerDefense.GameObjects.SpaceShips;

namespace CustomTowerDefense.Repository.Entities
{
    [Serializable]
    public class WaveElementDbEntity
    {
        public ushort RepeatNumber { get; set; }

        public ushort DelayBeforeCreation { get; set; }

        public PreciseObjectType SpaceShipType { get; set; }

        public WaveElement ToWaveElement()
        {
            Type spaceshipType;
            
            switch (SpaceShipType)
            {
                case PreciseObjectType.SmallScoutShip:
                    spaceshipType = typeof(SmallScoutShip);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"{SpaceShipType} is not handled a valid spaceship type.");
            }

            return new WaveElement
            {
                DelayBeforeCreation = DelayBeforeCreation,
                SpaceShipType = spaceshipType
            };
        }
    }
}