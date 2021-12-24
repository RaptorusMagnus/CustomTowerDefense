using System;

namespace CustomTowerDefense.Repository.Entities
{
    [Serializable]
    public class CoordinateEntity: BaseEntity
    {
        public ushort X;
        public ushort Y;
    }
}