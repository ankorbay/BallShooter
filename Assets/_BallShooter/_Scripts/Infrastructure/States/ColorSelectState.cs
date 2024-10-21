using _BallShooter._Scripts.UI;
using Infrastructure.Factory;
using Infrastructure.Services;
using Logic;
using UnityEngine;

namespace Infrastructure.States.Infrastructure.States
{
    public class ColorSelectState : IState
    {
        private const string ColorSelectScene = "ColorSelection";

        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;
        private readonly LoadingCurtain _loadingCurtain;
        private ColorSelectionController _colorSelectionController;

        public ColorSelectState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain, AllServices services)
        {
            _stateMachine = stateMachine;
            _loadingCurtain = loadingCurtain;
            _sceneLoader = sceneLoader;
            _services = services;
        }
        public void Enter()
        {
            _sceneLoader.Load(ColorSelectScene, onLoaded: OnSceneLoaded);
        }

        private void OnSceneLoaded()
        {
            Debug.Log($"Scene {ColorSelectScene} has been loaded");
            _services.Single<IGameFactory>().SpawnNetworkManager();
            _colorSelectionController = _services.Single<IGameFactory>().SpawnColorSelectionController();
            _colorSelectionController.OnColorConfirmed += OnColorConfirmed;
        }

        private void OnColorConfirmed()
        {
            _stateMachine.Enter<GameLobbyState>();
        }

        public void Exit()
        {
            _colorSelectionController.OnColorConfirmed -= OnColorConfirmed;
        }
    }
}