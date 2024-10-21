using _BallShooter._Scripts.Infrastructure.Services;
using Infrastructure.Factory;
using Infrastructure.Services;
using Infrastructure.States.Infrastructure.States;
using Logic;
using Services;
using Unity.Collections;
using UnityEngine;

namespace Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;
        private readonly LoadingCurtain _loadingCurtain;

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
            _stateMachine.Enter<ColorSelectState>();
        }

        public void Exit()
        {
           
        }
        
        private void RegisterServices()
        {
            _services.RegisterSingle<IAssetProvider>(new AssetProvider());
            _services.RegisterSingle<IStaticDataService>(new StaticDataService().Load());
            _services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAssetProvider>(), _services.Single<IStaticDataService>()));
            Debug.Log("Services have been registered");
        }
    }
}