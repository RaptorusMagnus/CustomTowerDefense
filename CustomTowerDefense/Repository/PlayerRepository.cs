using CustomTowerDefense.Repository.Entities;

namespace CustomTowerDefense.Repository
{
    public class PlayerRepository : BaseRepository
    {
        public PlayerRepository()
        {
            
        }

        public PlayerDbEntity GetPlayer()
        {
            return RepositoryData.Player;
        }
    }
}