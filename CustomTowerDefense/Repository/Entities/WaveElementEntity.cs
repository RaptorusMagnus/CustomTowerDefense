using System;
using CustomTowerDefense.GameObjects;

namespace CustomTowerDefense.Repository.Entities
{
    [Serializable]
    public class WaveElementEntity
    {
        public ushort RepeatNumber { get; set; }
        
        public ushort DelayBeforeCreation { get; set; }

        public PreciseObjectType SpaceShipType { get; set; }
    }
}