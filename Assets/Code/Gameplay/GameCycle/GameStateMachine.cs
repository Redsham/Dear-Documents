using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Gameplay.GameCycle
{
    public class GameStateMachine
    {
        [Inject] private readonly IObjectResolver m_Resolver;

        private IGameState              m_CurrentState;
        private UniTask<IGameState>     m_CurrentTask;
        private bool                    m_PreviousTaskIsRunning;
        private CancellationTokenSource m_CancellationTokenSource;


        public async UniTaskVoid SetState(IGameState toState)
        {
            IGameState nextState = toState;
            
            while(nextState != null && m_CancellationTokenSource is not { IsCancellationRequested: true })
            {
                // Cancel the current task if it is running
                if (m_PreviousTaskIsRunning)
                {
                    Debug.Log($"[GameStateMachine] Cancelling state {m_CurrentState.GetType().Name}");
                    
                    m_CancellationTokenSource?.Cancel();
                    await UniTask.WaitWhile(() => m_PreviousTaskIsRunning);
                }
            
                // Invoke the exit callback of the current state
                if(m_CurrentState is IOnStateExit exitState)
                    exitState.OnStateExit();
            
                // Dispose the previous cancellation token source and create a new one
                m_CancellationTokenSource?.Dispose();
                m_CancellationTokenSource = new CancellationTokenSource();
            
                // Inject the new state and start it
                m_CurrentState = nextState;
                m_Resolver.Inject(nextState);

                try
                {
                    // Invoke the enter callback of the new state
                    if (m_CurrentState is IOnStateEnter enterState)
                        enterState.OnStateEnter();
                
                    m_CurrentTask           = m_CurrentState.Handle(m_CancellationTokenSource.Token);
                    m_PreviousTaskIsRunning = true;
                    
                    Debug.Log($"[GameStateMachine] Entering state {m_CurrentState.GetType().Name}");
                
                    // Wait for the state to finish and get the next state
                    nextState               = await m_CurrentTask;
                }
                catch (OperationCanceledException) { /* Ignore the exception if the task was cancelled */ }
                catch(Exception ex)
                {
                    Debug.LogError($"Error while entering state {m_CurrentState.GetType().Name}: {ex}");
                }
                
                // The task has finished
                m_PreviousTaskIsRunning = false;
                Debug.Log($"[GameStateMachine] Exiting state {m_CurrentState.GetType().Name}");
                    
                // If the task was cancelled, do not proceed to the next state
                if(m_CancellationTokenSource.IsCancellationRequested) nextState = null;
            }
        }
    }
}