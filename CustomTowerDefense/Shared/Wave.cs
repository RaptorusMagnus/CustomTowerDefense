using System.Collections.Generic;

namespace CustomTowerDefense.Shared
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