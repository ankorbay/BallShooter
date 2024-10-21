using System.Threading.Tasks;
using _BallShooter._Scripts.UI;
using Services;

namespace Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        void SpawnNetworkManager();
        ColorSelectionController SpawnColorSelectionController();
        void CleanUp();
    }
}