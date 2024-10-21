using System.Threading.Tasks;
using _BallShooter._Scripts.Network;
using _BallShooter._Scripts.UI;
using Services;
using UnityEngine;

namespace Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        void SpawnNetworkManager();
        ColorSelectionController SpawnColorSelectionController();
        void CleanUp();
        LobbyGameUIController SpawnLobbyGameUIController();
        GameObject SpawnEnvironment();
    }
}