using System;

namespace CustomTowerDefense.Repository.Entities
{
    [Serializable]
    public class PlayerDbEntity
    {
        /// <summary>
        /// Current player level (the one that must be completed before moving to the next)
        /// </summary>
        public ushort Level { get; set; }
        
        /// <summary>
        /// Current Wave within current level.
        /// (the one that must be completed before moving to the next)
        /// </summary>
        public ushort WaveNumber { get; set; }

        public PlayerDbEntity()
        {
            Level = 1;
        }
    }
}