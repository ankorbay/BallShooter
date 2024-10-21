using System.Threading.Tasks;
using _BallShooter._Scripts.Infrastructure.Data;
using _BallShooter._Scripts.Infrastructure.Services;
using _BallShooter._Scripts.Network;
using _BallShooter._Scripts.UI;
using Infrastructure.Services;
using Infrastructure.States;
using Logic;
using Services;
using UnityEngine;

namespace Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly AllServices _services;
        private readonly IAssetProvider _assets;
        private readonly IStaticDataService _staticDataService;

        public GameFactory(GameStateMachine gameStateMachine, LoadingCurtain loadingCurtain, AllServices services)
        {
            _gameStateMachine = gameStateMachine;
            _services = services;
            _assets = _services.Single<IAssetProvider>();
            _staticDataService = _services.Single<IStaticDataService>();
        }

        public async Task SpawnNetworkManager()
        {
           NetworkManagerColorSelection networkManager = await _assets.Load<NetworkManagerColorSelection>(AssetAddress.NetworkManagerColorSelectionPath);
           networkManager.Configure(_staticDataService);
        }

        public async Task<ColorSelectionController> SpawnColorSelectionController()
        {
            GameObject colorSelectionPrefab = await _assets.Load<GameObject>(AssetAddress.ColorSelectionUI);
            ColorSelectionController colorSelectionController = colorSelectionPrefab.GetComponent<ColorSelectionController>();
            // colorSelectionController.Configure(_staticDataService);
            return colorSelectionController;
        }

        public void CleanUp()
        {
            _assets.CleanUp();
        }
    }
}
