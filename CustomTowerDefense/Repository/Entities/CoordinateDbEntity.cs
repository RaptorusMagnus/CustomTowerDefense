using System;

namespace CustomTowerDefense.Repository.Entities
{
    [Serializable]
    public class CoordinateDbEntity: BaseDbEntity
    {
        public ushort X;
        public ushort Y;
    }
}