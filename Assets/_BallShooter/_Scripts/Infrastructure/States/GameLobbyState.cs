using _BallShooter._Scripts.Infrastructure;
using _BallShooter._Scripts.Network;
using _BallShooter._Scripts.UI;
using Infrastructure.Factory;
using Infrastructure.Services;
using Logic;
using UnityEngine;

namespace Infrastructure.States
{
    public class GameLobbyState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IGameFactory _gameFactory;
        
        private GameObject _lobbyUI;
        private ColorSelectionController _colorSelectionController;
        private LobbyGameUIController _lobbyGameUIController;
        private GameObject _environment;

        public GameLobbyState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain, AllServices services)
        {
            _stateMachine = stateMachine;
            _loadingCurtain = loadingCurtain;
            _sceneLoader = sceneLoader;
            _services = services;
            _gameFactory = _services.Single<IGameFactory>();
        }
        public void Enter()
        {
            _sceneLoader.Load(SceneNames.LobbyGame, onLoaded:OnSceneLoaded);
        }

        public void Exit()
        {
            
        }

        private void OnSceneLoaded()
        {
            PrepareScene();
            NetworkManagerColorSelection.singleton.OnClientStartedEvent += OnClientStarted;
        }
        
        private void OnServerStopped()
        {
            _environment.SetActive(false);
        }

        private void OnClientStarted()
        {
            _environment.SetActive(true);
            _lobbyGameUIController.HideColorSelectorUI();
        }

        private void PrepareScene()
        {
            _gameFactory.GetSpawnBallPool();
            _environment = _gameFactory.SpawnEnvironment();
            _environment.SetActive(false);
            _colorSelectionController = _gameFactory.SpawnColorSelectionController();
            _lobbyGameUIController = _gameFactory.SpawnLobbyGameUIController();
            _colorSelectionController.OnColorConfirmed += OnColorConfirmed;
            _colorSelectionController.gameObject.SetActive(false);
        }

        private void OnColorConfirmed()
        {
            if(_lobbyGameUIController != null)
                _lobbyGameUIController.OpenLobbyUI();
        }
    }
}