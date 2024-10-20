using _BallShooter._Scripts.Infrastructure;
using Infrastructure.Services;
using Logic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string Initial = "Initial";

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
            _sceneLoader.Load(Initial, onLoaded: OnSceneLoaded);
        }

        private void OnSceneLoaded()
        {
            Debug.Log($"Scene {SceneNames.Initial} has been loaded");
        }

        public void Exit()
        {
           
        }
        
        private void RegisterServices()
        {
            // Add your services initialization points here
        }
    }
    
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
            _sceneLoader.Load(SceneNames.LobbyGame, LoadSceneMode.Additive, onLoaded: OnSceneLoaded);
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