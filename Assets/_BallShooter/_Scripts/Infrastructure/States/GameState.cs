using _BallShooter._Scripts.Infrastructure;
using Infrastructure.Services;
using Logic;
using UnityEngine.SceneManagement;

namespace Infrastructure.States
{
    public class GameState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;
        private readonly LoadingCurtain _loadingCurtain;

        public GameState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain, AllServices services)
        {
            _stateMachine = stateMachine;
            _loadingCurtain = loadingCurtain;
            _sceneLoader = sceneLoader;
            _services = services;
        }
        public void Enter()
        {
            _sceneLoader.Load(SceneNames.LobbyGame);
        }

        private void OnSceneLoaded()
        {
            // Add your logic here
        }

        public void Exit()
        {
            
        }

    }
}