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
            Debug.Log("Scene has been loaded");
        }

        public void Exit()
        {
            _loadingCurtain.Show();
        }
        
        private void RegisterServices()
        {
            // Add your services initialization points here
        }


    }
}