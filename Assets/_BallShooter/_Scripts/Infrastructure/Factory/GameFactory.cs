using _BallShooter._Scripts.Infrastructure.Data;
using _BallShooter._Scripts.Infrastructure.Services;
using _BallShooter._Scripts.Network;
using _BallShooter._Scripts.ObjectPool;
using _BallShooter._Scripts.UI;
using Services;
using UnityEngine;

namespace Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assets;
        private readonly IStaticDataService _staticDataService;

        private ColorSelectionController _colorSelectionController;
        private GameObject _lobbyUI;
        private GameObject _environment;
        private LobbyGameUIController _lobbyGameUIController;
        private PrefabPoolManager _prefabPoolManager;

        public GameFactory(IAssetProvider assetProvider, IStaticDataService staticDataService)
        {
            _assets = assetProvider;
            _staticDataService = staticDataService;
        }

        public PrefabPoolManager GetPrefabPoolManager()
        {
            if (_prefabPoolManager == null)
            {
                SpawnPrefabPoolManager();
            }
            
            return _prefabPoolManager;
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
            _colorSelectionController = colorSelectionPrefab.GetComponent<ColorSelectionController>();
            _colorSelectionController.Configure(_staticDataService);
            return _colorSelectionController;
        }

        public GameObject SpawnEnvironment()
        {
            _environment = _assets.Instantiate(AssetAddress.Environment);
            return _environment;
        }
        
        public void CleanUp()
        {
            _assets.CleanUp();
        }
        
        public PrefabPoolManager SpawnPrefabPoolManager()
        {
            GameObject prefabPoolManagerPrefab = _assets.Instantiate(AssetAddress.PrefabPoolManager);
            _prefabPoolManager = prefabPoolManagerPrefab.GetComponent<PrefabPoolManager>();
            return _prefabPoolManager;
        }

        public LobbyGameUIController SpawnLobbyGameUIController()
        {
            _lobbyGameUIController = _assets.Instantiate(AssetAddress.LobbyGameUIController).GetComponent<LobbyGameUIController>();
            _lobbyGameUIController.Configure(_colorSelectionController.gameObject, _environment);
            return _lobbyGameUIController;
        }
        
    }
}
