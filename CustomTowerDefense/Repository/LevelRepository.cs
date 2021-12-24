namespace CustomTowerDefense.Repository
{
    /// <summary>
    /// Utility class to load levels and check their content against some business rules.
    /// Do not put any level execution code here: this class must only load an check.
    ///
    ///   o     \ o /    \ /
    ///  /|\      |       |
    ///  / \     / \     /o\
    /// TODO: Currently some values and content are hard-coded, but they must ultimately come from a file 
    /// </summary>
    public class LevelRepository: BaseRepository
    {
        public LevelRepository()
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
        /// This method is in this loader because the value depends on the editable configuration file.
        /// So we don't know at design time what will be the highest level.
        /// </summary>
        /// <returns></returns>
        public ushort GetHighestAvailableLevel()
        {
            return 1;
        }
    }
}