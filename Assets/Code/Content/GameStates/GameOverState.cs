using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.GameCycle;
using UnityEngine;

namespace Content.GameStates
{
    public class GameOverState : IGameState
    {
        public UniTask<IGameState> Handle(CancellationToken cancellation)
        {
            Debug.Log("Game Over");
            
            // Exit from state machine
            return UniTask.FromResult<IGameState>(null);
        }
    }
}