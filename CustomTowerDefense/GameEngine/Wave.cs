using System.Collections.Generic;
using CustomTowerDefense.Shared;

namespace CustomTowerDefense.GameEngine
{
    public class Wave
    {
        public List<WaveElement> Elements { get; }

        public Wave()
        {
            Elements = new List<WaveElement>();
        }
    }
}