using System.Threading;
using Cysharp.Threading.Tasks;

namespace Gameplay.GameCycle
{
    public interface IGameState
    {
        /// <summary>
        /// Implement this method to perform actions during the state.
        /// </summary>
        UniTask<IGameState> Handle(CancellationToken cancellation);
    }
    
    public interface IOnStateEnter
    {
        /// <summary>
        /// Implement this method to perform actions when entering the state.
        /// </summary>
        void OnStateEnter();
    }
    public interface IOnStateExit
    {
        /// <summary>
        /// Implement this method to perform actions when exiting the state.
        /// </summary>
        void OnStateExit();
    }
}