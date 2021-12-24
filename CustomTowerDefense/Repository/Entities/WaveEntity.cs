using System;
using System.Collections.Generic;

namespace CustomTowerDefense.Repository.Entities
{
    [Serializable]
    public class WaveEntity
    {
        public List<WaveElementEntity> Elements { get; set; }
    }
}