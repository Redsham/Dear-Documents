using Content.GameStates;
using Gameplay.GameCycle;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameEntryPoint : IPostStartable
{
    [Inject] private GameStateMachine m_GameStateMachine;
        
    public void PostStart() => m_GameStateMachine.SetState(new WaitPersonState()).Forget();
}