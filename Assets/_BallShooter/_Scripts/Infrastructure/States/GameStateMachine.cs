using System;
using System.Collections.Generic;
using Infrastructure.Services;
using Logic;
using UnityEngine;

namespace Infrastructure.States
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type,IExitableState> _states;
        private IExitableState _activeState;
        
        public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain loadingCurtain, ICoroutineRunner coroutineRunner, AllServices services)
        {
            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, loadingCurtain, services),
            };
        }
        public void Enter<TState>() where TState : class, IState
        {
            TState state = ChangeState<TState>();
            state.Enter();
        }


        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
            Debug.Log($"You've entered {state}");
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
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