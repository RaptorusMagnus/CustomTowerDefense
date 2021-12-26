namespace CustomTowerDefense.Repository
{
    /// <summary>
    /// Utility class to load levels and check their content against some business rules.
    /// Do not put any level execution code here: this class must only load an check.
    /// </summary>
    public class LevelRepository: BaseRepository
    {
        /// <summary>
        /// Returns the highest level available in the game.
        /// The value depends on the editable configuration file.
        /// So we don't know at design time what will be the highest level.
        /// </summary>
        /// <returns></returns>
        public ushort GetHighestAvailableLevel()
        {
            return (ushort) (RepositoryData.Levels?.Count ?? 0);
        }
    }
}