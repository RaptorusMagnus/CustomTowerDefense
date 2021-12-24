using CustomTowerDefense.Repository.Entities;

namespace CustomTowerDefense.Repository
{
    public class PlayerRepository : BaseRepository
    {
        public PlayerRepository()
        {
            
        }

        public PlayerEntity GetPlayer()
        {
            return RepositoryData.Player;
        }
    }
}