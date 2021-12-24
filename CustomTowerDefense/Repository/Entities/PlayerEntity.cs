using System;

namespace CustomTowerDefense.Repository.Entities
{
    [Serializable]
    public class PlayerEntity
    {
        public ushort Level { get; set; }

        public PlayerEntity()
        {
            Level = 1;
        }
    }
}