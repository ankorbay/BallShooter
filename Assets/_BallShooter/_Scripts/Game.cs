using Infrastructure;
using Infrastructure.Services;
using Infrastructure.States;
using Logic;

public class Game
{
    public readonly GameStateMachine StateMachine;
    public static ICoroutineRunner CoroutineRunner;

    public Game(ICoroutineRunner coroutineRunner, LoadingCurtain loadingCurtain)
    {
        CoroutineRunner = coroutineRunner;
        StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), loadingCurtain, coroutineRunner, AllServices.Container);
    }
}