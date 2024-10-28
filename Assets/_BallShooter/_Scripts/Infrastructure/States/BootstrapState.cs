using _BallShooter._Scripts.Infrastructure.Services;
using _BallShooter._Scripts.UI;
using Infrastructure.Factory;
using Infrastructure.Services;
using Logic;
using Services;
using UnityEngine;

namespace Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;
        private readonly LoadingCurtain _loadingCurtain;
        private ColorSelectionController _colorSelectionController;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain, AllServices services)
        {
            _stateMachine = stateMachine;
            _loadingCurtain = loadingCurtain;
            _sceneLoader = sceneLoader;
            _services = services;
            
            RegisterServices();
        }
        public void Enter()
        {
            _services.Single<IGameFactory>().SpawnNetworkManager();
            _stateMachine.Enter<GameLobbyState>();
        }

        public void Exit()
        {
        }
        
        private void RegisterServices()
        {
            _services.RegisterSingle<IStaticDataService>(new StaticDataService());
            _services.Single<IStaticDataService>().Load();
            _services.RegisterSingle<IAssetProvider>(new AssetProvider());
            _services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAssetProvider>(), _services.Single<IStaticDataService>()));
            Debug.Log("Services have been registered");
        }
    }
}