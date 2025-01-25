using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Gameplay.GameCycle
{
    public class GameStateManager
    {
        [Inject] private readonly IObjectResolver m_Resolver;

        private IGameState              m_CurrentState;
        private UniTask<IGameState>     m_CurrentTask;
        private bool                    m_PreviousTaskIsRunning;
        private CancellationTokenSource m_CancellationTokenSource;


        public async UniTaskVoid SetState(IGameState initialState)
        {
            IGameState nextState = initialState;
            
            while(nextState != null)
            {
                // Cancel the current task if it is running
                if (m_PreviousTaskIsRunning)
                {
                    Debug.Log("Interrupting the current state");
                
                    m_CancellationTokenSource?.Cancel();
                    await UniTask.WaitWhile(() => m_PreviousTaskIsRunning);
                }
            
                if(m_CurrentState is IOnStateExit exitState)
                    exitState.OnStateExit();
            
                m_CancellationTokenSource?.Dispose();
                m_CancellationTokenSource = new CancellationTokenSource();
            
                m_CurrentState = nextState;
                m_Resolver.Inject(nextState);

                try
                {
                    if (m_CurrentState is IOnStateEnter enterState)
                        enterState.OnStateEnter();
                
                    m_CurrentTask           = m_CurrentState.Handle(m_CancellationTokenSource.Token);
                    m_PreviousTaskIsRunning = true;
                
                    nextState               = await m_CurrentTask;
                    m_PreviousTaskIsRunning = false;
                    
                    if(m_CancellationTokenSource.IsCancellationRequested)
                        break;
                }
                catch (OperationCanceledException ex)
                {
                    // Ignore the exception if the task was cancelled
                }
                catch(Exception ex)
                {
                    Debug.LogError($"Error while entering state {m_CurrentState.GetType().Name}: {ex}");
                }
            }
        }
    }
}