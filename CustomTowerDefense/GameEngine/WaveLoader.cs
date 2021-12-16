using CustomTowerDefense.GameObjects.SpaceShips;
using CustomTowerDefense.Shared;

namespace CustomTowerDefense.GameEngine
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
    public class WaveLoader
    {
        public WaveLoader()
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
        /// Returns the highest level available in the game.
        /// This method is in the wave loader because the value depends on the editable configuration file.
        /// So we don't know at design time what will be the highest level.
        /// </summary>
        /// <returns></returns>
        public ushort GetHighestAvailableLevel()
        {
            return 1;
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