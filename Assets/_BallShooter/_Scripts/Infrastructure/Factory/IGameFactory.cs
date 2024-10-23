using System.Threading.Tasks;
using _BallShooter._Scripts.Network;
using _BallShooter._Scripts.ObjectPool;
using _BallShooter._Scripts.UI;
using Services;
using UnityEngine;

namespace Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        public PrefabPoolManager GetPrefabPoolManager();
        
        void SpawnNetworkManager();
        ColorSelectionController SpawnColorSelectionController();
        PrefabPoolManager SpawnPrefabPoolManager();
        void CleanUp();
        LobbyGameUIController SpawnLobbyGameUIController();
        GameObject SpawnEnvironment();
    }
}