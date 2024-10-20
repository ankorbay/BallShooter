using Infrastructure.Services;
using Infrastructure.States;
using Logic;
using UnityEngine;

namespace Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        public LoadingCurtain loadingCurtain;
        private Game _game;
        
        public Game Game => _game;

        private void Awake()
        {
            _game = new Game(this, loadingCurtain);
            _game.StateMachine.Enter<BootstrapState>();
            Application.targetFrameRate = 60;
            
            DontDestroyOnLoad(this);
        }
    }
}

