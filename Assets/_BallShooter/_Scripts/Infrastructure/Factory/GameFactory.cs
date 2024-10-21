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
        private readonly IAssetProvider _assets;
        private readonly IStaticDataService _staticDataService;

        public GameFactory(IAssetProvider assetProvider, IStaticDataService staticDataService)
        {
            _assets = assetProvider;
            _staticDataService = staticDataService;
        }

        public void SpawnNetworkManager()
        {
           GameObject networkManagerPrefab = _assets.Instantiate(AssetAddress.NetworkManagerColorSelectionPath);
           NetworkManagerColorSelection networkManager = networkManagerPrefab.GetComponent<NetworkManagerColorSelection>();
           networkManager.Configure(_staticDataService);
        }

        public ColorSelectionController SpawnColorSelectionController()
        {
            GameObject colorSelectionPrefab = _assets.Instantiate(AssetAddress.ColorSelectionUI);
            ColorSelectionController colorSelectionController = colorSelectionPrefab.GetComponent<ColorSelectionController>();
            colorSelectionController.Configure(_staticDataService);
            return colorSelectionController;
        }

        public void CleanUp()
        {
            _assets.CleanUp();
        }
    }
}
