using System;
using System.Collections.Generic;
using Infrastructure.Services;
using Infrastructure.States.Infrastructure.States;
using Logic;
using UnityEngine;

namespace Infrastructure.States
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type,IExitableState> _states;
        private readonly LoadingCurtain _loadingCurtain;
        private IExitableState _activeState;

        public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain loadingCurtain, ICoroutineRunner coroutineRunner, AllServices services)
        {
            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, loadingCurtain, services),
                [typeof(ColorSelectState)] = new ColorSelectState(this, sceneLoader, loadingCurtain, services),
                [typeof(GameLobbyState)] = new GameLobbyState(this, sceneLoader, loadingCurtain, services),
            };
            _loadingCurtain = loadingCurtain;
        }
        public void Enter<TState>() where TState : class, IState
        {
            TState state = ChangeState<TState>();
            state.Enter();
            _loadingCurtain.Hide();
            Debug.Log($"You've entered {state}");
        }


        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
            _loadingCurtain.Hide();
            Debug.Log($"You've entered {state}");
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _loadingCurtain.Show();
            _activeState?.Exit();
            TState state = GetState<TState>();
            _activeState = state;
            
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState =>
            _states[typeof(TState)] as TState;

        public void Exit()
        {
            Debug.Log($"You exited {_activeState}");
        }
    }
}