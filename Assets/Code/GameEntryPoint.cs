using Content.GameStates;
using Gameplay.GameCycle;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameEntryPoint : IPostStartable
{
    [Inject] private GameStateManager m_GameStateManager;
        
    public void PostStart() => m_GameStateManager.SetState(new WaitPersonState()).Forget();
}