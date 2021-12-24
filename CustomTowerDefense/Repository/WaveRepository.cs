using CustomTowerDefense.GameEngine;
using CustomTowerDefense.GameObjects.SpaceShips;

namespace CustomTowerDefense.Repository
{
    /// <summary>
    /// Utility class to load waves and check their content against some business rules.
    /// Do not put any wave execution code here: this class must only load an check.
    ///
    ///   o     \ o /    \ /
    ///  /|\      |       |
    ///  / \     / \     /o\
    /// TODO: Currently some values and content are hard-coded, but they must ultimately come from a file 
    /// </summary>
    public class WaveRepository: BaseRepository
    {
        public WaveRepository()
        {
            //
            //   ,-~~-.___.
            //  / |  '     \
            // (  )         0
            //  \_/-, ,----'
            //     ====           // 
            //    /  \-'~;    /~~~(O)
            //   /  __/~|   /       |
            // =(  _____| (_________|
            //
            // TODO: load the file content here
        }

        /// <summary>
        /// Gets the number of waves defined for the received level
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public ushort GetNumberOfWaves(ushort level)
        {
            return 1;
        }

        public Wave GetWaveContent(ushort level, ushort waveNumber)
        {
            return new Wave
            {
                Elements =
                {
                    new WaveElement
                    {
                        DelayBeforeCreation = 0,
                        SpaceShipType = typeof(SmallScoutShip)
                    },
                    new WaveElement
                    {
                        DelayBeforeCreation = 0,
                        SpaceShipType = typeof(SmallScoutShip)
                    },
                    new WaveElement
                    {
                        DelayBeforeCreation = 0,
                        SpaceShipType = typeof(SmallScoutShip)
                    }
                }
            };
        }
    }
}