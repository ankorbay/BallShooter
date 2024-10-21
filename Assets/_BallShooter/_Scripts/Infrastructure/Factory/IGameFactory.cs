using System.Threading.Tasks;
using _BallShooter._Scripts.UI;
using Services;

namespace Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        public Task SpawnNetworkManager();
        
        public Task<ColorSelectionController> SpawnColorSelectionController();
        void CleanUp();
    }
}